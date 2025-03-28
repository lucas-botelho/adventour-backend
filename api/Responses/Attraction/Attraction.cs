namespace Adventour.Api.Responses.Attraction
{
    public class Attraction
    {
        public int Id { get; set; }
        public int IdCity { get; set; }
        public string Name { get; set; }
        public int AverageRating { get; set; }
        public string Description { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
    }
}
