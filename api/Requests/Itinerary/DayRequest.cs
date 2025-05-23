using System.Text.Json.Serialization;

namespace Adventour.Api.Requests.Itinerary
{
    public class DayRequest
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("dayNumber")]
        public int DayNumber { get; set; }

        [JsonPropertyName("timeslots")]
        public List<TimeSlotRequest>? Timeslots { get; set; }
    }
}
