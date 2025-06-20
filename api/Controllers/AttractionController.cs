using Adventour.Api.Responses.Country;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Attraction;
using Adventour.Api.Models.Database;
using Adventour.Api.Responses.Attractions;
using Microsoft.AspNetCore.Authorization;
using FirebaseAdmin.Auth;
using Adventour.Api.Services.DistanceCalculation.Interfaces;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionController : Controller
    {
        private readonly ILogger<CountryController> _logger;
        private readonly IAttractionRepository attractionRepository;
        private readonly IUserRepository userRepository;
        private readonly IGeoLocationService geoLocationService;

        public AttractionController(ILogger<CountryController> logger, IAttractionRepository attractionRepository, IUserRepository userRepository, IGeoLocationService geoLocationService)
        {
            _logger = logger;
            this.attractionRepository = attractionRepository;
            this.userRepository = userRepository;
            this.geoLocationService = geoLocationService;
        }

        [Authorize]
        [HttpGet("list/attractions")]
        public async Task<IActionResult> GetCountryActivities(string countryCode, [FromQuery] string oAuthId)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid country code"));
            }

            var user = await this.userRepository.GetUser(Request.Headers["Authorization"].ToString());
            if (string.IsNullOrEmpty(user?.OauthId))
                return BadRequest(new BaseApiResponse<string>("Invalid user."));

            var attractions = attractionRepository.GetBaseAttractionData(countryCode, user);

            return attractions is null
            ? NotFound(new BaseApiResponse<string>("Sorry, we dont have any attractions yet for this country."))
            : Ok(new BaseApiResponse<AttractionDetailsListResponse>(new AttractionDetailsListResponse(attractions), "Attractions found."));
        }

        [Authorize]
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

        [Authorize]
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

            Dictionary<int, AttractionInfoType> typesById = new();

            foreach (var info in infos)
            {
                var type = info.AttractionInfoType;
                if (!typesById.ContainsKey(type.Id))
                {
                    typesById[type.Id] = type;
                }
            }

            HashSet<AttractionInfoType> types = typesById.Values.ToHashSet();


            return Ok(new BaseApiResponse<AttractionInfoResponse>(new AttractionInfoResponse { AttractionInfos = infos, InfoTypes = types }, "Attraction found"));
        }

        [Authorize]
        [HttpPost("review/{attractionId}")]
        public IActionResult AddReview(string attractionId, [FromBody] AddReviewRequest request)
        {
            if (string.IsNullOrWhiteSpace(attractionId))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid attraction ID"));
            }

            bool success = attractionRepository.AddReview(Convert.ToInt32(attractionId), request);

            return Ok(new BaseApiResponse<string>("Attraction review added successufly", success));
        }

        [HttpGet("reviews/{attractionId}")]
        public IActionResult Reviews(string attractionId)
        {
            if (string.IsNullOrWhiteSpace(attractionId))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid attraction ID"));
            }
            var reviews = attractionRepository.GetAttractionReviews(Convert.ToInt32(attractionId));
            Attraction attraction = attractionRepository.GetAttraction(Convert.ToInt32(attractionId));


            return reviews is null
            ? NotFound(new BaseApiResponse<string>("Attraction not found"))
            : Ok(new BaseApiResponse<ReviewWithImagesResponse>(new ReviewWithImagesResponse(reviews, (double)(attraction?.AverageRating)), "Attraction found"));
        }

        [HttpGet("favorites")]
        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var token = await this.userRepository.GetUser(Request.Headers["Authorization"].ToString());

            if (string.IsNullOrEmpty(token?.OauthId))
                return BadRequest(new BaseApiResponse<string>("Invalid user."));

            var favorites = attractionRepository.GetFavorites(token.OauthId);

            return favorites is null || !favorites.Any()
            ? NotFound(new BaseApiResponse<string>("The user has no favorites."))
            : Ok(new BaseApiResponse<FavoritedAttractionListResponse>(new FavoritedAttractionListResponse(favorites), "Attractions found"));
        }

        [Authorize]
        [HttpGet("attraction/sorted/location")]
        public async Task<IActionResult> AttractionsByLocation([FromQuery] string latitute, [FromQuery] string longitude, [FromQuery] string countryCode, [FromQuery] bool descendant = true)
        {
            if (string.IsNullOrEmpty(latitute) || string.IsNullOrEmpty(longitude))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid location"));
            }

            var user = await this.userRepository.GetUser(Request.Headers["Authorization"].ToString());
            if (string.IsNullOrEmpty(user?.OauthId))
                return BadRequest(new BaseApiResponse<string>("Invalid user."));

            var attractions = attractionRepository.GetBaseAttractionData(countryCode, user);


            foreach (var attraction in attractions)
            {
                var result = await geoLocationService.GetDistanceInMetersAsync(
                     Convert.ToDouble(latitute),
                     Convert.ToDouble(longitude),
                     $"{attraction.Address}, {attraction.Country}"

                 );

                attraction.DistanceMeters = result.DistanceInMeters;
            }

            if (descendant)
                attractions = attractions.OrderByDescending(x => x.DistanceMeters);
            else
                attractions = attractions.OrderBy(x => x.DistanceMeters);


            return attractions is null
            ? NotFound(new BaseApiResponse<string>("Sorry, we dont have any attractions yet for this country."))
            : Ok(new BaseApiResponse<AttractionDetailsListResponse>(new AttractionDetailsListResponse(attractions), "Attractions found."));
        }


        [Authorize]
        [HttpGet("attraction/sorted/rating")]
        public async Task<IActionResult> AttractionsByRating([FromQuery] string countryCode, [FromQuery] bool descentant = true)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid country code"));
            }
            var user = await this.userRepository.GetUser(Request.Headers["Authorization"].ToString());
            if (string.IsNullOrEmpty(user?.OauthId))
                return BadRequest(new BaseApiResponse<string>("Invalid user."));

            var attractions = attractionRepository.GetBaseAttractionData(countryCode, user);

            if (attractions is null || !attractions.Any())
            {
                return NotFound(new BaseApiResponse<string>("Sorry, we dont have any attractions yet for this country."));
            }

            if (descentant)
                attractions = attractions.OrderByDescending(x => x.Rating);
            else
                attractions = attractions.OrderBy(x => x.Rating);

            return Ok(new BaseApiResponse<AttractionDetailsListResponse>(new AttractionDetailsListResponse(attractions), "Attractions found."));
        }


        [Authorize]
        [HttpGet("attraction/sorted/favorited")]
        public async Task<IActionResult> AttractionByFavorited([FromQuery] string countryCode, [FromQuery] bool descentant = true)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid country code"));
            }
            var user = await this.userRepository.GetUser(Request.Headers["Authorization"].ToString());
            if (string.IsNullOrEmpty(user?.OauthId))
                return BadRequest(new BaseApiResponse<string>("Invalid user."));

            var attractions = attractionRepository.GetBaseAttractionData(countryCode, user);

            if (attractions is null || !attractions.Any())
            {
                return NotFound(new BaseApiResponse<string>("Sorry, we dont have any attractions yet for this country."));
            }

            if (descentant)
                attractions = attractions.OrderByDescending(x => x.IsFavorited);
            else
                attractions = attractions.OrderBy(x => x.IsFavorited);
            return Ok(new BaseApiResponse<AttractionDetailsListResponse>(new AttractionDetailsListResponse(attractions), "Attractions found."));
        }

        [HttpPost("attraction")]
        public IActionResult AddAttraction([FromBody] AddAttractionRequest request)
        {
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid Duration"));
            }

            if (request.IdCountry <= 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid Country Id"));
            }

            var success = attractionRepository.AddAttraction(request);

            if (!success)
            {
                return StatusCode(500, new BaseApiResponse<string>("Failed to create attraction."));
            }

            return Ok(new BaseApiResponse<bool>("Attraction added successfully", success));
        }

        [HttpGet("infoTypes")]
        public IActionResult getInfoTypes()
        {
            var infoTypes = attractionRepository.GetInfoTypes();

            if (infoTypes == null)
            {
                return StatusCode(500, new BaseApiResponse<string>("Failed to get Info Types."));
            }

            return Ok(new BaseApiResponse<List<InfoTypeResponse>>(infoTypes, "Info Types retrieved successfully"));
        }
    }
}

