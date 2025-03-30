using Adventour.Api.Responses;
using Adventour.Api.Requests.TimeSlot;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using Adventour.Api.Services.TimeSlot;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        private readonly ILogger<TimeSlotController> logger;
        private readonly ITimeSlotService timeSlotService;

        public TimeSlotController(ILogger<TimeSlotController> logger, ITimeSlotService timeSlotService)
        {
            logger = logger;
            this.timeSlotService = timeSlotService;
        }

        [HttpPost("AddTimeSlot/")]
        public IActionResult AddTimeSlot([FromBody] AddTimeSlotRequest request)
        {
            if (request == null)
            {
                return BadRequest(new BaseApiResponse<string>("The request is empty"));
            }

            if (request.DayId <= 0 || request.AttractionId <= 0 || request.StartTime >= request.EndTime)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid data"));
            }

            try
            {
                int newTimeSlotId = timeSlotService.AddTimeSlot(request);
                return Ok(new BaseApiResponse<int>(newTimeSlotId, "TimeSlot added successfully"));
            }
            catch (NotFoundException ex)
            {
                return ex.Message.Contains("Attraction") ? NotFound(new BaseApiResponse<string>("Attraction doesn't exist!")) : NotFound(new BaseApiResponse<string>("Day doesn't exist!"));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error adding TimeSlot: {ex.Message}");
                return StatusCode(500, new BaseApiResponse<string>("Failed to add TimeSlot"));
            }
        }

        [HttpDelete("DeleteTimeSlot/{timeSlotId}")]
        public IActionResult DeleteTimeSlot(int timeSlotId)
        {
            if (timeSlotId <= 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid data"));
            }

            try
            {
                timeSlotService.DeleteTimeSlot(timeSlotId);
                return Ok(new BaseApiResponse<string>(timeSlotId.ToString(), "TimeSlot deleted successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new BaseApiResponse<string>("TimeSlot not found!"));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error deleting TimeSlot: {ex.Message}");
                return StatusCode(500, new BaseApiResponse<string>("An error occurred while deleting the TimeSlot"));
            }
        }
    }
}
