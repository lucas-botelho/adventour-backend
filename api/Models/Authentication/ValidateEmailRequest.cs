namespace Adventour.Api.Models.Authentication
{
    public class ValidateEmailRequest
    {
        public string Email { get; set; }
        public string Pin { get; set; }
    }
}