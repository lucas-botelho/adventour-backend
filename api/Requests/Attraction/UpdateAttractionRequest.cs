namespace Adventour.Api.Requests.Attraction
{
    public class UpdateAttractionRequest
    {
        public int AttractionId { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public int AverageRating { get; }
        public string Description { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
    }
}
