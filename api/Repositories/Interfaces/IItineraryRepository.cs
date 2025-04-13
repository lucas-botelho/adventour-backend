using Adventour.Api.Models.Itinerary;
using Adventour.Api.Requests.Itinerary;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IItineraryRepository
    {
        FullItineraryDetails AddItinerary(AddItineraryRequest request);
        FullItineraryDetails GetItineraryById(int itineraryId, string userId);
    }
}
