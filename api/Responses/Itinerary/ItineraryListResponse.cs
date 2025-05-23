namespace Adventour.Api.Responses.Itinerary
{
    public class ItineraryListResponse
    {
        public IEnumerable<FullItineraryDetails> Itineraries { get; set; }

        public ItineraryListResponse(IEnumerable<FullItineraryDetails> itinieraries)
        {
            this.Itineraries = itinieraries;
        }
    }
}
