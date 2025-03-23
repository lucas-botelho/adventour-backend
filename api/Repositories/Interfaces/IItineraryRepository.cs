using Adventour.Api.Responses.Itinerary;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IItineraryRepository
    {
        ItineraryResponse GetItineraryById(int itineraryId);
    }
}
