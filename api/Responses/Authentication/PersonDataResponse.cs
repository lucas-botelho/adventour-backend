namespace Adventour.Api.Responses.Authentication
{
    public class PersonDataResponse
    {
        public string OauthId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
        public string PhotoUrl { get; set; }

    }
}
