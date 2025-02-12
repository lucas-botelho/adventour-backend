using Adventour.Api.Services.FileUpload.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;

namespace Adventour.Api.Services.FileUpload
{
    public class CloudinaryService : IFileUploadService
    {
        private readonly ILogger<CloudinaryService> logger;
        private const string logHeader = "## CloudinaryService ##: ";
        public CloudinaryService(ILogger<CloudinaryService> logger)
        {
            this.logger = logger;
        }
        private Cloudinary Cloudinary { get; set; }
        public CloudinaryService()
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            this.Cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            this.Cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var result = await Cloudinary.UploadAsync(uploadParams);

            var uploadFailed = result.Error != null;

            if (uploadFailed)
            {
                logger.LogError($"{logHeader} {result.Error.Message}");
                return string.Empty;
            }

            return result.SecureUrl.AbsoluteUri;

          
        }
    }
}
