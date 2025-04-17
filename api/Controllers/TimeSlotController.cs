using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.TimeSlot;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Adventour.Api.Responses.TimeSlots;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotController : Controller
    {
        private readonly ILogger<TimeSlotController> _logger;
        private readonly ITimeSlotRepository timeSlotRepository;

        public TimeSlotController(ILogger<TimeSlotController> logger, ITimeSlotRepository timeSlotRepository)
        {
            _logger = logger;
            this.timeSlotRepository = timeSlotRepository;
        }

        [HttpPost()]
        public IActionResult AddTimeSlot([FromBody] AddTimeSlotRequest request)
        {
            try
            {
                if (request.DayId <= 0 || request.AttractionId <= 0 || request.StartTime >= request.EndTime)
                {
                    return BadRequest(new BaseApiResponse<string>("Invalid data to create the TimeSlot."));
                }

                var result = timeSlotRepository.AddTimeSlot(request);

                return Ok(new BaseApiResponse<BasicTimeSlotDetails>(result, "TimeSlot created successfully."));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error when creating TimeSlot."));
            }
        }

        [HttpDelete("remove")]
        public IActionResult RemoveTimeSlot(int timeSlotId)
        {
            try
            {
                if (timeSlotId <= 0)
                {
                    return BadRequest(new BaseApiResponse<string>("Invalid TimeSlot ID."));
                }

                var wasDeleted = timeSlotRepository.RemoveTimeSlot(timeSlotId);

                return Ok(new BaseApiResponse<BasicTimeSlotDetails>("TimeSlot successfully deleted."));
                
            }
            catch(AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error when deleting TimeSlot."));
            }
        }

        [HttpPut()]
        public IActionResult UpdateTimeSlot([FromBody] UpdateTimeSlotRequest request)
        {
            try
            {
                if (request.TimeSlotId <= 0 || request.AttractionId <= 0 || request.DayId <= 0 || request.StartTime >= request.EndTime)
                {
                    return BadRequest(new BaseApiResponse<string>("Invalid data to update TimeSlot."));
                }

                var result = timeSlotRepository.UpdateTimeSlot(request);

                return Ok(new BaseApiResponse<BasicTimeSlotDetails>(result, "TimeSlot updated successfully."));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error when updating TimeSlot."));
            }
        }

    }
}
