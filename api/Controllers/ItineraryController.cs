using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Repositories;
using Adventour.Api.Requests.Itinerary;
using Adventour.Api.Exceptions;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Itinerary;
using Adventour.Api.Models.Database;
using Azure.Core;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryController : Controller
    {
        private readonly ILogger<ItineraryController> _logger;
        private readonly IItineraryRepository itineraryRepository;
        private readonly IUserRepository userRepository;
        private readonly ICountryRepository countryRepository;

        public ItineraryController(ILogger<ItineraryController> logger, IItineraryRepository itineraryRepository, IUserRepository userRepository, ICountryRepository countryRespository)
        {
            _logger = logger;
            this.itineraryRepository = itineraryRepository;
            this.userRepository = userRepository;
            this.countryRepository = countryRespository;
        }

        [HttpGet()]
        public IActionResult GetItineraryById([FromQuery] int itineraryId, [FromQuery] string userId)
        {
            try
            {
                if (itineraryId <= 0)
                {
                    return BadRequest(new BaseApiResponse<string>("Invalid Data!"));
                }

                var itinerary = itineraryRepository.GetItineraryById(itineraryId, userId);

                if (itinerary == null)
                {
                    return NotFound(new BaseApiResponse<string>("Itinerary not found."));
                }

                return Ok(new BaseApiResponse<FullItineraryDetails>(itinerary, "Itinerary retrieved successfully"));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error when retrieving Itinerary."));
            }
        }

        [HttpGet("itinerary")]
        public async Task<IActionResult> Itenerary([FromQuery] string countryCode)
        {
            var user = await this.userRepository.GetUser(Request.Headers["Authorization"].ToString());
            if (string.IsNullOrEmpty(user?.OauthId))
                return BadRequest(new BaseApiResponse<string>("Invalid user."));


            var country = countryRepository.GetCountry(countryCode);
            try
            {
                IEnumerable<FullItineraryDetails> itineraries = itineraryRepository.GetUserItineraries(user, country);

                return Ok(new BaseApiResponse<ItineraryListResponse>(new ItineraryListResponse(itineraries), "Itineraries retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error when retrieving Itineraries."));
            }
        }

        [HttpPost("itinerary")]
        public async Task<IActionResult> CreateItinerary([FromBody] ItineraryRequest body)
        {
            var user = await this.userRepository.GetUser(Request.Headers["Authorization"].ToString());
            if (string.IsNullOrEmpty(user?.OauthId))
                return BadRequest(new BaseApiResponse<string>("Invalid user."));


            var itinerary = itineraryRepository.AddItinerary(body, user);

            if (itinerary == null)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid Itinerary data."));
            }

            return Ok(new BaseApiResponse<Itinerary>(itinerary, "Itinerary created successfully"));
        }
    }
}
