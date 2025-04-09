namespace Adventour.Api.Requests.Attraction
{
    public class AddToFavoriteRequest
    {
        public int AttractionId { get; set; }
        public string UserId { get; set; }

    }
}
