namespace Adventour.Api.Responses
{
    public class BaseApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
