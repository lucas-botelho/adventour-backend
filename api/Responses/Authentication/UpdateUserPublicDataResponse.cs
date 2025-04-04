namespace Adventour.Api.Responses.Authentication
{
    public class UpdateUserPublicDataResponse
    {
        public UpdateUserPublicDataResponse(bool isUpdated)
        {
            this.Updated = isUpdated;
        }
        public bool Updated { get; set; }
    }
}
