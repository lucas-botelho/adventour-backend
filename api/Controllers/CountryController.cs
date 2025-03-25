using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Country;
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

            try
            {
                var country = countryRepository.GetCountry(code);

                return Ok(new BaseApiResponse<CountryResponse>(country, "Country found"));
            }
            catch (Exception)
            {
                return NotFound(new BaseApiResponse<string>("Country not found"));
            }

        }

        [HttpGet("list/countries")]
        public IActionResult GetCountries([FromQuery]int page, [FromQuery]int pageSize, [FromQuery] string continent)
        {
            if (page < 1 || pageSize < 1 || string.IsNullOrWhiteSpace(continent))
            {
                return BadRequest(new BaseApiResponse<string>("Invalid query string."));
            }

            try
            {
                var countries = countryRepository.GetCountries(page, pageSize, continent);
                return Ok(new BaseApiResponse<CountriesListResponse>(new CountriesListResponse(countries), "Countries found"));
            }
            catch (Exception)
            {
                return NotFound(new BaseApiResponse<string>("Countries not found"));
            }
        }
    }
}
