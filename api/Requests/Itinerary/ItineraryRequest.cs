using Adventour.Api.Models.Database;
using System.Text.Json.Serialization;

namespace Adventour.Api.Requests.Itinerary
{
    public class ItineraryRequest
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("days")]
        public List<DayRequest>? Days { get; set; }
    }
}
