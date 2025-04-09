using Adventour.Api.Repositories;
using Adventour.Api.Responses.Country;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Attraction;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionController : Controller
    {

        private readonly ILogger<CountryController> _logger;
        private readonly IAttractionRepository attractionRepository;

        public AttractionController(ILogger<CountryController> logger, IAttractionRepository attractionRepository)
        {
            _logger = logger;
            this.attractionRepository = attractionRepository;
        }
       
        [HttpGet("list/attractions")]
        public IActionResult GetCountryActivities(string countryCode, [FromQuery] string oAuthId)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid country code"));
            }

            var attractions = attractionRepository.GetBaseAttractionData(countryCode, oAuthId);

            return attractions is null
            ? NotFound(new BaseApiResponse<string>("Sorry, we dont have any attractions yet for this country."))
            : Ok(new BaseApiResponse<BasicAttractionListResponse>(new BasicAttractionListResponse(attractions), "Attractions found."));
        }

        [HttpPost("favorite")]
        public IActionResult AddToFavorites([FromBody] AddToFavoriteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || request.AttractionId < 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid attraction ID"));
            }
           
            var success = attractionRepository.AddToFavorites(request.AttractionId, request.UserId);
            return Ok(new BaseApiResponse<string>("Attraction added to favorites", success));
        }

        [HttpPost("favorite/remove")]
        public IActionResult RemoveFavorite([FromBody] AddToFavoriteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || request.AttractionId < 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid attraction ID"));
            }

            var success = attractionRepository.RemoveFavorite(request.AttractionId, request.UserId);
            return Ok(new BaseApiResponse<string>("Attraction added to favorites", success));
        }
    }
}
