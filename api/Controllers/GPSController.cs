using Adventour.Api.Exceptions;
using Adventour.Api.Models.Geolocation;
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
        private readonly IGeoLocationService tomTomService;

        public GPSController(ILogger<GPSController> logger, IGeoLocationService tomTomService)
        {
            this.logger = logger;
            this.tomTomService = tomTomService;
        }

        [HttpGet("distance")]
        public async Task<IActionResult> GetDistance([FromQuery] double originLat, [FromQuery] double originLon, [FromQuery] double destLat, [FromQuery] double destLon)
        {
            if (originLat == destLat && originLon == destLon)
            {
                return BadRequest(new BaseApiResponse<string>("Source and Destination cannot be the same."));
            }

            try
            {
                var distance = await tomTomService.GetDistanceInMetersAsync(originLat, originLon, destLat, destLon);
                return Ok(new BaseApiResponse<DistanceResult>(distance, "Distance calculated successfully."));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error calculating distance."));
            }
        }

        [HttpGet("geocode")]
        public async Task<IActionResult> Geocode(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid address."));
            }
            try
            {
                var result = await tomTomService.AddressToGeoCode(address);
                return Ok(new BaseApiResponse<GeocodeResult>(result, "Geocode successful."));
            }
            catch (AppException ex)
            {
                return StatusCode(ex.StatusCode, new BaseApiResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseApiResponse<string>("Unexpected error during geocoding."));
            }
        }
    }

}