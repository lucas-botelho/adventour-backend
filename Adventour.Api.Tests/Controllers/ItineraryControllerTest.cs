using Adventour.Api.Controllers;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Itinerary;
using Adventour.Api.Models.Database;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Itinerary;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Adventour.Api.Requests.Attraction;

namespace Adventour.Api.Tests.Controllers
{
    public class ItineraryControllerTests
    {
        private readonly Mock<IItineraryRepository> _itineraryRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ICountryRepository> _countryRepoMock;
        private readonly Mock<IAttractionRepository> _attractionRepoMock;
        private readonly Mock<ILogger<ItineraryController>> _loggerMock;
        private readonly ItineraryController _controller;

        public ItineraryControllerTests()
        {
            _itineraryRepoMock = new Mock<IItineraryRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _countryRepoMock = new Mock<ICountryRepository>();
            _attractionRepoMock = new Mock<IAttractionRepository>();
            _loggerMock = new Mock<ILogger<ItineraryController>>();

            _controller = new ItineraryController(
                _loggerMock.Object,
                _itineraryRepoMock.Object,
                _userRepoMock.Object,
                _countryRepoMock.Object
            );

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Request.Headers["Authorization"] = "Bearer test-token";
        }

        [Fact]
        public async Task CreateItinerary_WithTwoAttractions_ReturnsItineraryWithTwoActivities()
        {
            var testUser = new Person
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                OauthId = "test-oauth-id"
            };

            var request = new ItineraryRequest
            {
                Name = "Fim de semana na Serra",
                Days = new List<DayRequest>
        {
            new DayRequest
            {
                DayNumber = 1,
                Timeslots = new List<TimeSlotRequest>
                {
                    new TimeSlotRequest
                    {
                        StartTime = "09:00",
                        EndTime = "11:00",
                        AttractionId = 123
                    },
                    new TimeSlotRequest
                    {
                        StartTime = "14:00",
                        EndTime = "16:00",
                        AttractionId = 456
                    }
                }
            }
        }
            };

            var expectedItinerary = new Itinerary
            {
                Id = 10,
                Title = "Fim de semana na Serra",
                CreatedAt = DateTime.UtcNow,
                UserId = testUser.Id,
                Days = new List<Day>
        {
            new Day
            {
                Id = 1,
                DayNumber = 1,
                Timeslots = new List<Timeslot>
                {
                    new Timeslot
                    {
                        Id = 1,
                        StartTime = DateTime.Today.AddHours(9),
                        EndTime = DateTime.Today.AddHours(11),
                        AttractionId = 123,
                        Attraction = new Attraction
                        {
                            Id = 123,
                            Name = "Serra da Estrela Adventure"
                        }
                    },
                    new Timeslot
                    {
                        Id = 2,
                        StartTime = DateTime.Today.AddHours(14),
                        EndTime = DateTime.Today.AddHours(16),
                        AttractionId = 456,
                        Attraction = new Attraction
                        {
                            Id = 456,
                            Name = "Caminhada no Gerês"
                        }
                    }
                }
            }
        }
            };

            _userRepoMock.Setup(repo => repo.GetUser(It.IsAny<string>()))
                         .ReturnsAsync(testUser);

            _itineraryRepoMock.Setup(repo => repo.AddItinerary(request, testUser))
                              .Returns(expectedItinerary);

            var result = await _controller.CreateItinerary(request);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;

            var response = okResult!.Value.Should().BeOfType<BaseApiResponse<Itinerary>>().Subject;
            response.Success.Should().BeTrue();
            response.Data.Days.Should().HaveCount(1);

            var timeslots = response.Data.Days.First().Timeslots.ToList();
            timeslots.Should().HaveCount(2);
            timeslots.Select(t => t.Attraction.Name).Should().Contain("Serra da Estrela Adventure");
            timeslots.Select(t => t.Attraction.Name).Should().Contain("Caminhada no Gerês");
        }

    }
}
