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

        [HttpPost("AddDay/")]
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

        [HttpDelete("DeleteDay/{dayId}")]
        public IActionResult DeleteDay(int dayId)
        {
            if (dayId <= 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid data"));
            }

            try
            {
                var success = dayRepository.DeleteDay(dayId);

                if (success)
                {
                    return Ok(new BaseApiResponse<string>(dayId.ToString(), "Day deleted successfully"));
                }
                else
                {
                    return NotFound(new BaseApiResponse<string>("Day not found or could not be deleted"));
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new BaseApiResponse<string>("An error occurred while deleting the day"));
            }
        }
    }
}