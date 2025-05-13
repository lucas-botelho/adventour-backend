namespace Adventour.Api.Responses.Attractions
{
    public class FavoritedAttractionDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double AverageRating { get; set; }
        public string CountryName { get; set; }
        public string Image { get; internal set; }
    }
}
