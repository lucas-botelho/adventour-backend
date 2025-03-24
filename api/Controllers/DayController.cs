using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Day;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Requests.Day;
using Adventour.Api.Repositories;
using Adventour.Api.Services.Day;
using SendGrid.Helpers.Errors.Model;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayController : ControllerBase
    {
        private readonly ILogger<DayController> _logger;
        private readonly IDayService dayService;

        public DayController(ILogger<DayController> logger, IDayService dayService)
        {
            _logger = logger;
            this.dayService = dayService;
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
                int newDayId = dayService.AddDay(request);
                return Ok(new BaseApiResponse<int>(newDayId, "Day added successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new BaseApiResponse<string>("Day doesn't exist!"));
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
                dayService.DeleteDay(dayId);
                return Ok(new BaseApiResponse<string>(dayId.ToString(), "Day deleted successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new BaseApiResponse<string>("Day not found!"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting day: {ex.Message}");
                return StatusCode(500, new BaseApiResponse<string>("An error occurred while deleting the day"));
            }
        }
    }
}