using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Services.FileUpload.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Files;
using Adventour.Api.Requests.Files;

namespace Adventour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly IFileUploadService fileUploadService;
        private readonly ILogger<FilesController> logger;
        private const string logHeader = "## FilesController ##: ";
        public FilesController(IFileUploadService fileUploadService, ILogger<FilesController> logger)
        {
            this.fileUploadService = fileUploadService;
            this.logger = logger;
        }

        [HttpPost("upload")]
        //[Authorize]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
        {
            var isValidImage = request.File?.Length > 0;
            if (isValidImage)
            {
                var result = await fileUploadService.UploadFileAsync(request.File!);

                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, new BaseApiResponse<string>("File upload failed"));
                }

                //todo: update da conta do user com o link da imagem

                return Ok(new BaseApiResponse<FileUploadResponse>(
                    new FileUploadResponse()
                    {
                        PublicUrl = result,
                    },
                    "File uploaded successfully")
                );
            }

            return BadRequest(new BaseApiResponse<string>("Submitted image is not valid."));
        }
    }
}
