using Adventour.Api.Data;
using Adventour.Api.Models.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses.Attractions;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<BasicAttractionDetails> GetBaseAttractionData(string countryCode, string userId)
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
                    return Enumerable.Empty<BasicAttractionDetails>();
                }

                var attractions = selectedCountry.Attractions;
                var user = db.Person.FirstOrDefault(p => p.OauthId != null && p.OauthId.Equals(userId));
                if (user is null)
                {
                    logger.LogError($"{logHeader} User code not found.");
                    return Enumerable.Empty<BasicAttractionDetails>();
                }

                List<BasicAttractionDetails> baseAttractions = attractions.Where(a => a.AttractionImages.Count > 0).Select(
                    attraction => new BasicAttractionDetails
                    {
                        Id = attraction.Id,
                        Name = attraction.Name,
                        Description = attraction.ShortDescription,
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

            return Enumerable.Empty<BasicAttractionDetails>();

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

        public Attraction GetAttraction(int id)
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
    }
}
