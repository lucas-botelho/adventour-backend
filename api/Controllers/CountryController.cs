using Adventour.Api.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;

        public CountryController(ILogger<CountryController> logger)
        {
            _logger = logger;
        }

        [HttpGet("country/{code}")]
        public IActionResult GetCountry(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != 2)
            {
                return BadRequest(new BaseApiResponse<string>()
                {
                    Data = null,
                    Message = "Invalid country code",
                    Success = false,
                    Errors = new List<string>() { "Invalid country code" }
                });
            }

            return Ok("Check the console logs.");


        }
    }
}
