namespace Adventour.Api.Responses.Attractions
{
    public class FavoritedAttractionListResponse
    {
        public FavoritedAttractionListResponse(IEnumerable<FavoritedAttractionDetails> Attractions)
        {
            this.Attractions = Attractions.ToList();
        }

        public IEnumerable<FavoritedAttractionDetails> Attractions { get; set; }
    }
}
