using Adventour.Api.Repositories;
using Adventour.Api.Responses.Country;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Repositories.Interfaces;

namespace Adventour.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionController : Controller
    {

        private readonly ILogger<CountryController> _logger;
        private readonly IAttractionRepository attractionRepository;

        public AttractionController(ILogger<CountryController> logger, IAttractionRepository attractionRepository)
        {
            _logger = logger;
            this.attractionRepository = attractionRepository;
        }
       
        [HttpGet("list/attractions")]
        public IActionResult GetCountryActivities(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid country code"));
            }

            var attractions = attractionRepository.GetBaseAttractionData(countryCode);

            return attractions is null
            ? NotFound(new BaseApiResponse<string>("Sorry, we dont have any attractions yet for this country."))
            : Ok(new BaseApiResponse<BasicAttractionListResponse>(new BasicAttractionListResponse(attractions), "Attractions found."));
        }
    }
}
