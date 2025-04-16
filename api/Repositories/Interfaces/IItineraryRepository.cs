using Adventour.Api.Requests.Itinerary;
using Adventour.Api.Responses.Itinerary;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IItineraryRepository
    {
        FullItineraryDetails AddItinerary(AddItineraryRequest request);
        FullItineraryDetails GetItineraryById(int itineraryId, string userId);
    }
}
