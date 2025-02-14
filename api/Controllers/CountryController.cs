using Adventour.Api.Models.Country;
using Adventour.Api.Repositories.Interfaces;
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
        private readonly ICountryRepository countryRepository;

        public CountryController(ILogger<CountryController> logger, ICountryRepository countryRepository)
        {
            _logger = logger;
            this.countryRepository = countryRepository;
        }

        [HttpGet("country/{code}")]
        public IActionResult GetCountry(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != 2)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid country code"));
            }

            var country = countryRepository.GetCountry(code);

            return Ok(new BaseApiResponse<Country>(country, "Country found"));
        }
    }
}
