using Adventour.Api.Responses.Day;
using Adventour.Api.Requests.Day;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IDayRepository
    {
        int AddDay(AddDayRequest request);
        bool DeleteDay(int dayId);
        DayResponse GetDayById(int dayId);
    }
}
