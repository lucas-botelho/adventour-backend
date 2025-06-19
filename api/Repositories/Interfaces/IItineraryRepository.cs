using Adventour.Api.Models.Database;
using Adventour.Api.Requests.Itinerary;
using Adventour.Api.Responses.Itinerary;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IItineraryRepository
    {
        Itinerary AddItinerary(ItineraryRequest request, Person user);
        FullItineraryDetails GetItineraryById(int itineraryId, string userId);
        IEnumerable<FullItineraryDetails> GetUserItineraries(Person user, Country country);
        bool DeleteItinerary(int itineraryId, Person user);
    }
}
