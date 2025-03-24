using Adventour.Api.Requests.Day;

namespace Adventour.Api.Services.Day
{
    public interface IDayService
    {
        int AddDay(AddDayRequest request);
        void DeleteDay(int dayId);
    }
}