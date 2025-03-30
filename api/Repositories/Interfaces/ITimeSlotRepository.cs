using Adventour.Api.Requests.TimeSlot;
using Adventour.Api.Responses.TimeSlot;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ITimeSlotRepository
    {
        int AddTimeSlot(AddTimeSlotRequest request);
        void DeleteTimeSlot(int timeSlotId);
        //List<TimeSlot> GetTimeSlotsByDayId(int dayId);
        TimeSlot GetTimeSlotById(int timeSlotId);
    }
}
