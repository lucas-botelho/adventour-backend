using System.Text.Json.Serialization;

namespace Adventour.Api.Requests.Itinerary
{
    public class TimeSlotRequest
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("start_time")]
        public string StartTime { get; set; } = string.Empty;

        [JsonPropertyName("end_time")]
        public string EndTime { get; set; } = string.Empty;

        [JsonPropertyName("attraction_id")]
        public int? AttractionId { get; set; }
    }
}
