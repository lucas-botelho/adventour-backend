namespace Adventour.Api.Responses
{
    public class BaseApiResponse<T>
    {
        public BaseApiResponse(T data, string message)           
        {
            Data = data;
            Success = true;
            Message = message;
        }
        public BaseApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
        }
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
