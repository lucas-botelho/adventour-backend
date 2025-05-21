using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.TimeSlot;
using Adventour.Api.Responses;
using Adventour.Api.Responses.TimeSlots;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Adventour.Api.Responses.Day;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayController : Controller
    {
        private readonly ILogger<DayController> _logger;
        private readonly IDayRepository dayRepository;

        public DayController(ILogger<DayController> logger, IDayRepository dayRepository)
        {
            _logger = logger;
            this.dayRepository = dayRepository;
        }

        [HttpPost()] 
        public IActionResult AddDay([FromBody] AddDayRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new BaseApiResponse<string>("Empty request."));
                }

                if (request.itineraryId <= 0)
                {
                    return BadRequest(new BaseApiResponse<string>("Invalid data to create Day."));
                }


                var day = dayRepository.AddDay(request);

                return Ok(new BaseApiResponse<BasicDayDetails>(day, "Day created successfully"));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error when creating Day."));
            }
        }

        [HttpDelete()]
        public IActionResult RemoveDay(int dayId)
        {
            try
            {
                if (dayId <= 0)
                {
                    return BadRequest(new BaseApiResponse<string>("Invalid data to remove Day."));
                }

                dayRepository.RemoveDay(dayId);

                return Ok(new BaseApiResponse<BasicDayDetails>("Day removed successfully"));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error when removing Day."));
            }
        }

    }
}
