using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Itinerary;
using Microsoft.AspNetCore.Mvc;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private readonly ILogger<ItineraryController> _logger;
        private readonly IItineraryRepository itineraryRepository;

        public ItineraryController (ILogger<ItineraryController> logger, IItineraryRepository itineraryRepository)
        {
            _logger = logger;
            this.itineraryRepository = itineraryRepository;
        }
        
        [HttpGet("Itinerary/{itineraryId}")]
        public IActionResult GetItineraryById(int itineraryId) {
            if (itineraryId < 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid itinerary id"));
            }

            try
            {
                var itinerary = itineraryRepository.GetItineraryById(itineraryId);

                if (itinerary == null) {
                    return NotFound(new BaseApiResponse<string>("Itinerary not found"));
                }

                return Ok(new BaseApiResponse<ItineraryResponse>(itinerary, "Itinerary found"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching itinerary: {ex.Message}");
                return StatusCode(500, new BaseApiResponse<string>("An unexpected error occurred"));
            }
        }
        
    }
}
