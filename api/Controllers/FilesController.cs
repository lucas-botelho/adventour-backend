using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Adventour.Api.Services.FileUpload.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Files;
using Microsoft.AspNetCore.Authorization;

namespace Adventour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly IFileUploadService fileUploadService;
        public FilesController(IFileUploadService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }

        [HttpPost("upload")]
        //[Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var isValidImage = file?.Length > 0;
            if (isValidImage)
            {
                try
                {
                    var result = await fileUploadService.UploadFileAsync(file!);

                    return Ok(new BaseApiResponse<FileUploadResponse>()
                    {
                        Data = new FileUploadResponse()
                        {
                            filePublicReference = result,
                        },
                        Success = true,
                        Message = "File uploaded successfully",
                    });
                }
                catch (Exception)
                {
                    //todo add logs
                    return StatusCode(500, new BaseApiResponse<string>()
                    {
                        Data = null,
                        Success = false,
                        Message = "File upload failed",
                    });
                }
            }

            return BadRequest(new BaseApiResponse<string>()
            {
                Data = null,
                Success = false,
                Message = "Submitted image is not valid.",
            });
        }
    }
}
