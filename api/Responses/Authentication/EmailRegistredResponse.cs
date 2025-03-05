namespace Adventour.Api.Responses.Authentication
{
    public class EmailRegistredResponse
    {
        public EmailRegistredResponse(bool isRegistred)
        {
            this.IsRegistred = isRegistred;
        }
        public bool IsRegistred { get; set; }

    }
}
