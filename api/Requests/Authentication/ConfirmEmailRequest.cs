namespace Adventour.Api.Requests.Authentication
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }
        public string Pin { get; set; }
    }
}