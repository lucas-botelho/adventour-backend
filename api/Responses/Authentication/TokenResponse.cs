namespace Adventour.Api.Responses.Authentication
{
    public class TokenResponse
    {
        private int expiresIn = 3600;

        public string Token { get; set; }

        public int ExpiresIn { get => expiresIn; set => expiresIn = value; }
        public string UserId { get; set; }
    }
}
