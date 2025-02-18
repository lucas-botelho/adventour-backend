namespace Adventour.Api.Models.Authentication
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }
        public string Pin { get; set; }
    }
}