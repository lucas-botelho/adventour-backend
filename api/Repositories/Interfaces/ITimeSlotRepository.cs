using Adventour.Api.Requests.TimeSlot;
using Adventour.Api.Responses.TimeSlots;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ITimeSlotRepository
    {
        TimeSlotDetails? AddTimeSlot(AddTimeSlotRequest request);
        bool RemoveTimeSlot(int idTimeSlot);
        TimeSlotDetails UpdateTimeSlot(UpdateTimeSlotRequest request);
    }
}
