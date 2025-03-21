using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Day;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Requests.Day;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayController : ControllerBase
    {
        private readonly ILogger<DayController> _logger;
        private readonly IDayRepository dayRepository;

        public DayController(ILogger<DayController> logger, IDayRepository dayRepository)
        {
            _logger = logger;
            this.dayRepository = dayRepository;
        }

        [HttpGet("itinerary/{itineraryId}")]
        public IActionResult GetDaysByItineraryId(int itineraryId)
        {
            if (itineraryId <= 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid itinerary id"));
            }

            try
            {
                var days = dayRepository.GetDaysByItineraryId(itineraryId);

                if (days == null || days.Count == 0)
                {
                    return NotFound(new BaseApiResponse<string>("No days found for this itinerary"));
                }

                return Ok(new BaseApiResponse<List<DayResponse>>(days, "Days retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching days: {ex.Message}");
                return StatusCode(500, new BaseApiResponse<string>("An unexpected error occurred"));
            }
        }

        [HttpPost]
        public IActionResult AddDay([FromBody] AddDayRequest request)
        {
            if (request.ItineraryId <= 0 || request.DayNumber <= 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid data"));
            }

            try
            {
                int newDayId = dayRepository.AddDay(request);
                return Ok(new BaseApiResponse<int>(newDayId, "Day added successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding day: {ex.Message}");
                return StatusCode(500, new BaseApiResponse<string>("Failed to add day"));
            }
        }
    }
}