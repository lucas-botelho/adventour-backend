using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Adventour.Api.Services.FileUpload.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Files;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var isValidImage = file?.Length > 0;
            if (isValidImage)
            {
                var result = await fileUploadService.UploadFileAsync(file!);

                if (string.IsNullOrEmpty(result))
                {
                    return StatusCode(500, new BaseApiResponse<string>("File upload failed"));
                }

                return Ok(new BaseApiResponse<FileUploadResponse>(
                    new FileUploadResponse()
                    {
                        filePublicReference = result,
                    },
                    "File uploaded successfully")
                );
            }

            return BadRequest(new BaseApiResponse<string>("Submitted image is not valid."));
        }
    }
}
