using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Repositories.Interfaces; 
using Adventour.Api.Repositories; 
using Adventour.Api.Requests.Itinerary; 
using Adventour.Api.Models.Itinerary; 
using Adventour.Api.Exceptions; 
using Adventour.Api.Responses; 

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryController : Controller
    {
        private readonly ILogger<ItineraryController> _logger;
        private readonly IItineraryRepository itineraryRepository;

        public ItineraryController(ILogger<ItineraryController> logger, IItineraryRepository itineraryRepository)
        {
            _logger = logger;
            this.itineraryRepository = itineraryRepository;
        }

        [HttpPost("create")]
        public IActionResult AddItinerary([FromBody] AddItineraryRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseApiResponse<string>("Empty request."));
                }

                if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Title))
                {
                    return BadRequest(new BaseApiResponse<string>("Invalid data to create Itinerary."));
                }

                var itinerary = itineraryRepository.AddItinerary(request);

                return Ok(new BaseApiResponse<FullItineraryDetails>(itinerary, "Itinerary created successfully"));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error when creating Itinerary."));
            }
        }


        [HttpGet("get")]
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
    }
}
