namespace Adventour.Api.Requests.Files
{
    public class MultipleFilesUploadRequest
    {
        public List<IFormFile> Files { get; set; } = new();
    }
}
