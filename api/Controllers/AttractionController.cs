using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Attraction;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;

namespace Adventour.Api.Controllers
{
    [Route("api/attractions")]
    [ApiController]
    public class AttractionController : ControllerBase
    {
        private readonly IAttractionRepository attractionRepository;
        private readonly ILogger<AttractionController> logger;
        private const string logHeader = "## AttractionController ##: ";

        public AttractionController(IAttractionRepository attractionRepository, ILogger<AttractionController> logger)
        {
            this.attractionRepository = attractionRepository;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult GetAttractionById(int id)
        {
            logger.LogInformation($"{logHeader} Fetching attraction with Id={id}");

            var attraction = attractionRepository.GetAttractionById(id);

            if (attraction == null)
            {
                logger.LogWarning($"{logHeader} Attraction with Id={id} not found.");
                return NotFound(new { success = false, message = "Attraction not found" });
            }

            return Ok(new { success = true, data = attraction, message = "Attraction found" });
        }

        [HttpPost("AddAttraction")]
        public IActionResult AddAttraction([FromBody] AddAttractionRequest request)
        {
            if(request == null)
            {
                return BadRequest(new BaseApiResponse<string>("The request is empty"));
            }

            if (request.CityId <= 0)
            {
                return BadRequest(new BaseApiResponse<string>("Invalid City for the attraction!"));
            }

            try
            {
                var newAttractionId = attractionRepository.AddAttraction(request);
                return Ok(new BaseApiResponse<int>(newAttractionId, "Attraction added successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new BaseApiResponse<string>("City doesn't exist!"));
            }
            catch (Exception ex) {
                logger.LogError($"Error adding attraction: {ex.Message}");
                return StatusCode(500, new BaseApiResponse<string>("Failed to add Attraction"));
            }

            
        }

    }
}
