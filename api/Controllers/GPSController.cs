using Adventour.Api.Exceptions;
using Adventour.Api.Responses;
using Adventour.Api.Responses.DistanceCalculation;
using Adventour.Api.Services.DistanceCalculation.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Adventour.Api.Controllers
{
    public class GPSController : Controller
    {
        private readonly ILogger<GPSController> logger;
        private const string logHeader = "## GPSController ##: ";
        private readonly ITomTomService tomTomService;

        public GPSController(ILogger<GPSController> logger, ITomTomService tomTomService) 
        { 
            this.logger = logger;
            this.tomTomService = tomTomService;
        }

        [HttpGet("distance")]
        public async Task<IActionResult> GetDistance([FromQuery] double originLat, [FromQuery] double originLon, [FromQuery] double destLat, [FromQuery] double destLon)
        {
            if (originLat == destLat && originLon == destLon)
            {
                return BadRequest(new BaseApiResponse<string>("Origem e destino não podem ser iguais."));
            }

            try
            {
                var distance = await tomTomService.GetDistanceInMetersAsync(originLat, originLon, destLat, destLon);
                return Ok(new BaseApiResponse<DistanceResult>(distance, "Distância calculada com sucesso."));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Erro inesperado ao calcular a distância."));
            }
        }



    }
}
