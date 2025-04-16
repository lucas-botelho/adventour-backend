using Adventour.Api.Responses.Day;

namespace Adventour.Api.Responses.Itinerary
{
    public class FullItineraryDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<BasicDayDetails> Days { get; set; } = new List<BasicDayDetails>();
    }
}
