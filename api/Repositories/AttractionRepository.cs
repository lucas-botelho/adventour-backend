using Adventour.Api.Data;
using Adventour.Api.Models.Attraction;
using Adventour.Api.Models.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Attraction;
using Adventour.Api.Responses.Attractions;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Adventour.Api.Repositories
{
    public class AttractionRepository : IAttractionRepository
    {
        private readonly AdventourContext db;
        private readonly ILogger<CountryRepository> logger;
        private const string logHeader = "## AttractionRepository ##: ";
        public AttractionRepository(ILogger<CountryRepository> logger, AdventourContext context)
        {
            this.logger = logger;
            db = context;
        }

        public IEnumerable<AttractionDetails> GetBaseAttractionData(string countryCode, Person user)
        {
            try
            {
                var selectedCountry = db.Country
                    .Where(country => country.Code.Equals(countryCode.ToUpper()))
                    .Include(country => country.Attractions)
                    .ThenInclude(attraction => attraction.AttractionImages)
                    .FirstOrDefault();

                if (selectedCountry is null)
                {
                    logger.LogError($"{logHeader} Country code not found.");
                    return Enumerable.Empty<AttractionDetails>();
                }

                var attractions = selectedCountry.Attractions;
                if (user is null)
                {
                    logger.LogError($"{logHeader} User code not found.");
                    return Enumerable.Empty<AttractionDetails>();
                }

                List<AttractionDetails> baseAttractions = attractions.Where(a => a.AttractionImages.Count > 0).Select(
                    attraction => new AttractionDetails
                    {
                        Id = attraction.Id,
                        Name = attraction.Name,
                        Country = attraction.Country.Name,
                        Rating = attraction.AverageRating,
                        Address = $"{attraction.AddressOne}, {attraction.AddressTwo}",
                        ShortDescription = attraction.ShortDescription,
                        LongDescription = attraction.LongDescription,
                        IsFavorited = db.Favorites.Any(favorite => favorite.AttractionId == attraction.Id && favorite.UserId.Equals(user.Id)),
                        AttractionImages = attraction.AttractionImages
                            .Where(i => i.IsMain)
                            .Select(
                                img => new AttractionImages
                                {
                                    PictureRef = img.PictureRef,
                                }
                            ).ToList(),
                    }).ToList();

                return baseAttractions;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return Enumerable.Empty<AttractionDetails>();

        }

        public bool AddToFavorites(int attractionId, string userId)
        {
            try
            {
                Person? user = db.Person.FirstOrDefault(person => person.OauthId != null && person.OauthId.Equals(userId));
                if (user is not null)
                {
                    var attractionExists = db.Attraction.Any(attraction => attraction.Id.Equals(attractionId));
                    var favoriteExists = db.Favorites.Any(favorite => favorite.AttractionId == attractionId && favorite.UserId == user.Id);

                    if (attractionExists && !favoriteExists)
                    {
                        db.Favorites.Add(new Favorites
                        {
                            AttractionId = attractionId,
                            UserId = user.Id
                        });

                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return false;
        }

        public bool RemoveFavorite(int attractionId, string userId)
        {
            try
            {
                Person? user = db.Person.FirstOrDefault(person => person.OauthId != null && person.OauthId.Equals(userId));
                var favorite = db.Favorites.FirstOrDefault(f => f.AttractionId == attractionId && f.UserId.Equals(user.Id));
                if (favorite is not null)
                {
                    db.Remove(favorite);
                    db.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return false;
        }

        public Attraction? GetAttractionWithImages(int id)
        {
            try
            {
                var attraction = db.Attraction
                    .Include(a => a.AttractionImages)
                    .FirstOrDefault(a => a.Id == id);

                return attraction;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return null;
        }

        public IEnumerable<AttractionInfo>? GetAttractionInfo(int id)
        {
            try
            {
                var attraction = db.Attraction
                   .Include(a => a.AttractionInfos)
                   .ThenInclude(i => i.AttractionInfoType)
                   .Where(a => a.Id == id)
                   .FirstOrDefault();

                return attraction?.AttractionInfos.Select(
                    info => new AttractionInfo
                    {
                        AttractionId = info.AttractionId,
                        AttractionInfoTypeId = info.AttractionInfoTypeId,
                        Title = info.Title,
                        Description = info.Description,
                        AttractionInfoType = new AttractionInfoType
                        {
                            Id = info.AttractionInfoType.Id,
                            TypeTitle = info.AttractionInfoType.TypeTitle,
                        }

                    }).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return null;
        }

        public bool AddReview(int id, AddReviewRequest data)
        {
            using var transaction = db.Database.BeginTransaction();

            try
            {
                var user = db.Person.FirstOrDefault(p => p.OauthId != null && p.OauthId.Equals(data.OAuthId));
                var rating = db.Rating.FirstOrDefault(r => r.Value == data.Rating);

                if (user != null && rating != null)
                {
                    var review = db.Review.Add(new Review
                    {
                        AttractionId = id,
                        UserId = user.Id,
                        Rating = rating,
                        Comment = data.Review,
                        Title = data.Title
                    });
                    db.SaveChanges();


                    db.ReviewImages.AddRange(data.ImagesUrls.Select(url => new ReviewImages
                    {
                        PictureRef = url,
                        ReviewId = review.Entity.Id
                    }));

                    db.SaveChanges();

                    var allRatings = db.Review
                        .Where(r => r.AttractionId == id)
                        .Select(r => r.Rating.Value)
                        .ToList();

                    if (allRatings.Any())
                    {
                        var average = Math.Round(allRatings.Average(), 1); // 1 casa decimal

                        var attraction = db.Attraction.FirstOrDefault(a => a.Id == id);
                        if (attraction != null)
                        {
                            attraction.AverageRating = average;
                            db.SaveChanges();
                        }
                    }

                    transaction.Commit();
                    return true;
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} ${ex.Message}");
            }

            transaction.Rollback();
            return false;

        }

        public IEnumerable<ReviewWithImages> GetAttractionReviews(int attractionId)
        {
            try
            {
                var reviews = db.Review.Where(r => r.AttractionId == attractionId)
                    .Include(r => r.Person)
                    .Include(r => r.Rating)
                    .Select(
                        r => new Review
                        {
                            Rating = r.Rating,
                            Id = r.Id,
                            Person = new Person
                            {
                                Username = r.Person.Username,
                                PhotoUrl = r.Person.PhotoUrl,
                                OauthId = r.Person.OauthId,
                            },
                            Comment = r.Comment,
                            Title = r.Title,
                        }
                    )
                    .ToList();

                var images = db.ReviewImages
                    .Where(i => reviews.Select(r => r.Id).Contains(i.ReviewId))
                    .ToList();


                var result = new List<ReviewWithImages>();

                foreach (var review in reviews)
                {
                    var reviewResponse = new ReviewWithImages
                    {
                        Review = review,
                        Images = images.Where(i => i.ReviewId == review.Id).ToList(),
                    };

                    result.Add(reviewResponse);
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return null;
        }

        public IEnumerable<FavoritedAttractionDetails> GetFavorites(string oAuthId)
        {
            var favorites = db.Favorites
                .Include(f => f.Attraction)
                .ThenInclude(a => a.AttractionImages)
                .Where(f => f.Person.OauthId != null && f.Person.OauthId.Equals(oAuthId))
                .Select(f => new FavoritedAttractionDetails
                {
                    Id = f.Attraction.Id,
                    Name = f.Attraction.Name,
                    AverageRating = f.Attraction.AverageRating,
                    CountryName = f.Attraction.Country.Name,
                    Image = f.Attraction.AttractionImages
                        .Where(i => i.IsMain)
                        .Select(i => i.PictureRef)
                        .FirstOrDefault() ?? string.Empty,
                })
                .ToList();

            return favorites;
        }

        public Attraction GetAttraction(int id) => db.Attraction.FirstOrDefault(a => a.Id == id);

        public bool AddAttraction(AddAttractionRequest data)
        {
            using var transaction = db.Database.BeginTransaction();

            try
            {
                var country = db.Country.FirstOrDefault(c => c.Id == data.IdCountry);
                if (country == null)
                    return false;

                var attraction = new Attraction
                {
                    Name = data.Name,
                    ShortDescription = data.ShortDescription,
                    LongDescription = data.LongDescription,
                    DurationMinutes = data.DurationMinutes,
                    AddressOne = data.AddressOne,
                    AddressTwo = data.AddressTwo,
                    CountryId = country.Id
                };

                db.Attraction.Add(attraction);
                db.SaveChanges();

                if (data.Infos != null && data.Infos.Any())
                {
                    foreach (var info in data.Infos)
                    {
                        db.AttractionInfo.Add(new AttractionInfo
                        {
                            AttractionId = attraction.Id,
                            AttractionInfoTypeId = info.IdAttractionInfoType,
                            Title = info.Title,
                            Description = info.Description
                        });
                    }
                }

                if (data.Images != null && data.Images.Any())
                {
                    foreach (var image in data.Images)
                    {
                        db.AttractionImages.Add(new AttractionImages
                        {
                            AttractionId = attraction.Id,
                            PictureRef = image.PictureRef,
                            IsMain = image.IsMain
                        });
                    }
                }

                db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                transaction.Rollback();
                return false;
            }
        }

        public List<InfoTypeResponse> GetInfoTypes()
        {
            try
            {
                return db.AttractionInfoType
                    .Select(it => new InfoTypeResponse
                    {
                        Id = it.Id,
                        TypeTitle = it.TypeTitle
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                return new List<InfoTypeResponse>();
            }
        }

    }
}
