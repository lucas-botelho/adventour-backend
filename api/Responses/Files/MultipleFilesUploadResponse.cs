namespace Adventour.Api.Responses.Files
{
    public class MultipleFilesUploadResponse
    {
        public MultipleFilesUploadResponse(IEnumerable<FileUploadResponse> data)
        {
            files = data.ToList();
        }
        public IEnumerable<FileUploadResponse> files { get; set; }
    }
}
