using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.TimeSlot;
using Adventour.Api.Responses;
using Adventour.Api.Models.TimeSlots;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("create")]
        public IActionResult AddTimeSlot([FromBody] AddTimeSlotRequest request)
        {
            try
            {
                if (request.DayId <= 0 || request.StartTime >= request.EndTime)
                {
                    return BadRequest(new BaseApiResponse<string>("Dados inválidos para criar o TimeSlot."));
                }

                var result = timeSlotRepository.AddTimeSlot(request);

                if (result is null)
                {
                    return NotFound(new BaseApiResponse<string>("Não foi possível criar o TimeSlot."));
                }

                return Ok(new BaseApiResponse<BasicTimeSlotDetails>(result, "TimeSlot criado com sucesso."));
            }
            catch(Exception ex)
            {
                return BadRequest(new BaseApiResponse<string>("Erro ao criar o TimeSlot: " + ex.Message));
            }
        }


        [HttpDelete("remove")]
        public IActionResult RemoveTimeSlot(int timeSlotId)
        {
            if (timeSlotId <= 0)
            {
                return BadRequest(new BaseApiResponse<string>("Id de TimeSlot inválido."));
            }

            var wasDeleted = timeSlotRepository.RemoveTimeSlot(timeSlotId);

            if (!wasDeleted)
            {
                return NotFound(new BaseApiResponse<string>("Não foi possível criar o TimeSlot."));
            }

            return Ok(new BaseApiResponse<BasicTimeSlotDetails>("TimeSlot apagado com sucesso."));
        }
    }
}
