using Dapper;

namespace Adventour.Api.Responses.Itinerary
{
    public class ItineraryResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatedAt { get; set; }
    }
}