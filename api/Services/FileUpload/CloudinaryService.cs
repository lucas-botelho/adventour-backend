using Adventour.Api.Services.FileUpload.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;

namespace Adventour.Api.Services.FileUpload
{
    public class CloudinaryService : IFileUploadService
    {

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
            var uploadResult = await Cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error == null)
            {
                return uploadResult.SecureUrl.AbsoluteUri;
            }

            throw new Exception(uploadResult.Error.Message);
        }
    }
}
