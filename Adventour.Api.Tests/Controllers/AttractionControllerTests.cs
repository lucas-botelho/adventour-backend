using Adventour.Api.Controllers;
using Adventour.Api.Models.Attraction;
using Adventour.Api.Models.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Attraction;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Attractions;
using Adventour.Api.Services.DistanceCalculation.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Adventour.Api.Tests.Controllers
{
    public class AttractionControllerTests
    {
        private readonly Mock<IAttractionRepository> _attractionRepoMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IGeoLocationService> _geoLocationServiceMock = new();
        private readonly Mock<ILogger<CountryController>> _loggerMock = new();

        private readonly AttractionController _controller;

        public AttractionControllerTests()
        {
            _controller = new AttractionController(
                _loggerMock.Object,
                _attractionRepoMock.Object,
                _userRepoMock.Object,
                _geoLocationServiceMock.Object
            );
        }

        [Fact]
        public void GetAttraction_ValidId_ReturnsAttractionDetails()
        {
            var attractionId = 1;
            var attraction = new Attraction
            {
                Id = attractionId,
                Name = "Serra da Estrela Adventure",
                LongDescription = "Experiência de montanha inesquecível.",
                ShortDescription = "Natureza e aventura.",
                AttractionImages = new List<AttractionImages>
                {
                    new AttractionImages { PictureRef = "serra.jpg", IsMain = true }
                }
            };

            _attractionRepoMock
                .Setup(repo => repo.GetAttractionWithImages(attractionId))
                .Returns(attraction);

            var result = _controller.GetAttraction(attractionId.ToString());

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            var response = okResult!.Value.Should().BeOfType<BaseApiResponse<Attraction>>().Subject;

            response.Message.Should().Be("Attraction found");
            response.Data.Name.Should().Be("Serra da Estrela Adventure");
            response.Data.AttractionImages.Should().NotBeEmpty();
            response.Data.LongDescription.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void AddToFavorites_ValidRequest_ReturnsSuccess()
        {
            var request = new AddToFavoriteRequest
            {
                UserId = "user-123",
                AttractionId = 1
            };

            _attractionRepoMock
                .Setup(repo => repo.AddToFavorites(request.AttractionId, request.UserId))
                .Returns(true);

            var result = _controller.AddToFavorites(request);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            var response = okResult!.Value.Should().BeOfType<BaseApiResponse<string>>().Subject;
            response.Message.Should().Be("Attraction added to favorites");
            response.Success.Should().BeTrue();
        }

        [Fact]
        public void RemoveFavorite_ValidRequest_ReturnsSuccess()
        {
            var request = new AddToFavoriteRequest
            {
                UserId = "user-123",
                AttractionId = 1
            };

            _attractionRepoMock
                .Setup(repo => repo.RemoveFavorite(request.AttractionId, request.UserId))
                .Returns(true);

            var result = _controller.RemoveFavorite(request);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            var response = okResult!.Value.Should().BeOfType<BaseApiResponse<string>>().Subject;

            response.Success.Should().BeTrue();
            response.Message.Should().Be("Attraction added to favorites");
        }

        [Fact]
        public void AddReview_ValidReview_ReturnsSuccess()
        {
            var attractionId = "1";

            var reviewRequest = new AddReviewRequest
            {
                OAuthId = "user-123",
                Rating = 5,
                Title = "Fantástico",
                Review = "Incrível!",
                ImagesUrls = new List<string>()
            };

            _attractionRepoMock.Setup(repo =>
                repo.AddReview(int.Parse(attractionId), reviewRequest)
            ).Returns(true);

            // Act
            var result = _controller.AddReview(attractionId, reviewRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            var response = okResult!.Value.Should().BeOfType<BaseApiResponse<string>>().Subject;
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Attraction review added successufly");
        }

        [Fact]
        public void Reviews_ValidAttractionId_ReturnsAllReviews()
        {
            var attractionId = "1";

            var reviews = new List<ReviewWithImages>
    {
        new ReviewWithImages
        {
            Review = new Review
            {
                Id = 1,
                Title = "Espetacular",
                Comment = "Incrível!",
                Rating = new Rating { Value = 5 },
                Person = new Person
                {
                    Username = "maria",
                    PhotoUrl = "https://example.com/maria.jpg",
                    OauthId = "user-maria"
                }
            },
            Images = new List<ReviewImages>
            {
                new ReviewImages { PictureRef = "https://example.com/maria1.jpg" }
            }
        },
        new ReviewWithImages
        {
            Review = new Review
            {
                Id = 2,
                Title = "Belo local",
                Comment = "Perfeito para um piquenique",
                Rating = new Rating { Value = 4 },
                Person = new Person
                {
                    Username = "joao",
                    PhotoUrl = "https://example.com/joao.jpg",
                    OauthId = "user-joao"
                }
            },
            Images = new List<ReviewImages>()
        },
        new ReviewWithImages
        {
            Review = new Review
            {
                Id = 3,
                Title = "Recomendo",
                Comment = "Paisagem deslumbrante!",
                Rating = new Rating { Value = 5 },
                Person = new Person
                {
                    Username = "ana",
                    PhotoUrl = "https://example.com/ana.jpg",
                    OauthId = "user-ana"
                }
            },
            Images = new List<ReviewImages>
            {
                new ReviewImages { PictureRef = "https://example.com/ana1.jpg" },
                new ReviewImages { PictureRef = "https://example.com/ana2.jpg" }
            }
        }
    };

            _attractionRepoMock.Setup(repo => repo.GetAttractionReviews(1))
                .Returns(reviews);

            _attractionRepoMock.Setup(repo => repo.GetAttraction(1))
                .Returns(new Attraction { AverageRating = 4.7 });

            var result = _controller.Reviews(attractionId);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            var response = okResult!.Value.Should()
                .BeOfType<BaseApiResponse<ReviewWithImagesResponse>>()
                .Subject;

            response.Success.Should().BeTrue();
            response.Data.AverageRating.Should().Be(4.7);
            response.Data.Reviews.Should().HaveCount(3);

            var responseReviews = response.Data.Reviews.ToList();

            var r1 = responseReviews[0];
            r1.Review.Comment.Should().Be("Incrível!");
            r1.Review.Person.Username.Should().Be("maria");
            r1.Images.Should().ContainSingle(i => i.PictureRef == "https://example.com/maria1.jpg");

            var r2 = responseReviews[1];
            r2.Review.Comment.Should().Be("Perfeito para um piquenique");
            r2.Review.Person.Username.Should().Be("joao");
            r2.Images.Should().BeEmpty();

            var r3 = responseReviews[2];
            r3.Review.Comment.Should().Be("Paisagem deslumbrante!");
            r3.Review.Person.Username.Should().Be("ana");
            r3.Images.Should().HaveCount(2);
            r3.Images.Select(i => i.PictureRef).Should().Contain("https://example.com/ana1.jpg");
            r3.Images.Select(i => i.PictureRef).Should().Contain("https://example.com/ana2.jpg");
        }
    }
}
