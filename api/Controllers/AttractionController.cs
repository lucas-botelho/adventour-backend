using Adventour.Api.Responses.Country;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Attraction;
using Adventour.Api.Models.Database;
using Adventour.Api.Responses.Attractions;
using Microsoft.AspNetCore.Authorization;
using Adventour.Api.Models.Attraction;

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

        [HttpGet("attraction/{id}")]
        public IActionResult GetAttraction(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid attraction ID"));
            }
            var attraction = attractionRepository.GetAttractionWithImages(Convert.ToInt32(id));
            return attraction is null
            ? NotFound(new BaseApiResponse<string>("Attraction not found"))
            : Ok(new BaseApiResponse<Attraction>(attraction, "Attraction found"));
        }

        [HttpGet("info/{id}")]
        public IActionResult AttractionInfo(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid attraction ID"));
            }
            var infos = attractionRepository.GetAttractionInfo(Convert.ToInt32(id));

            if (infos is null || !infos.Any())
            {
                return NotFound(new BaseApiResponse<string>("Attraction not found"));
            }

            HashSet<AttractionInfoType> types = new HashSet<AttractionInfoType>();

            foreach (var info in infos)
                types.Add(info.AttractionInfoType);

            return Ok(new BaseApiResponse<AttractionInfoResponse>(new AttractionInfoResponse { AttractionInfos = infos, InfoTypes = types }, "Attraction found"));
        }

        [HttpPost("review/{attractionId}")]
        [Authorize]
        public IActionResult AddReview(string attractionId, [FromBody] AddReviewRequest request)
        {
            if (string.IsNullOrWhiteSpace(attractionId))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid attraction ID"));
            }

            bool success = attractionRepository.AddReview(Convert.ToInt32(attractionId), request);

            return Ok(new BaseApiResponse<string>("Attraction review added successufly", success));
        }

        [HttpGet("review/{attractionId}")]
        public IActionResult GetReviews(string attractionId)
        {
            if (string.IsNullOrWhiteSpace(attractionId))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid attraction ID"));
            }
            var reviews = attractionRepository.GetAttractionReviews(Convert.ToInt32(attractionId));
            return reviews is null
            ? NotFound(new BaseApiResponse<string>("Attraction not found"))
            : Ok(new BaseApiResponse<ReviewWithImagesResponse>(new ReviewWithImagesResponse(reviews), "Attraction found"));
        }
    }
}
