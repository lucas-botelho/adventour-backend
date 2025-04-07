using Adventour.Api.Models;
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

            var country = countryRepository.GetCountry(code);

            return country is null
            ? NotFound(new BaseApiResponse<string>("Country not found"))
            : Ok(new BaseApiResponse<Country>(country, "Country found"));
        }

        [HttpGet("list/countries")]
        public IActionResult GetCountries(
                [FromQuery] int pageSize,
                [FromQuery] string continent,
                [FromQuery] string selectedCountryCode,
                [FromQuery] int page = 0
            )
        {
            if (pageSize < 1 || string.IsNullOrWhiteSpace(continent) || selectedCountryCode.Length != 2)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid query string parameters."));
            }

            int total = 0;
            var countries = countryRepository.GetCountries(
                continent,
                selectedCountryCode.ToUpper(),
                pageSize,
                page,
                out total
            );

            if (countries is null || !countries.Any())
            {
                return NotFound(new BaseApiResponse<string>("Countries not found"));
            }

            return Ok(new BaseApiResponse<CountriesListResponse>(new CountriesListResponse(countries, total), "Countries found"));
        }
    }
}
