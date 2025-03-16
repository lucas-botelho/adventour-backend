using Adventour.Api.Responses.Day;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IDayRepository
    {
        List<DayResponse> GetDaysByItineraryId(int itineraryId);
        //int AddDay(AddDayRequest request);
    }
}
