using Adventour.Api.Requests.TimeSlot;
using Adventour.Api.Responses.TimeSlots;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ITimeSlotRepository
    {
        BasicTimeSlotDetails? AddTimeSlot(AddTimeSlotRequest request);
        bool RemoveTimeSlot(int idTimeSlot);
        BasicTimeSlotDetails UpdateTimeSlot(UpdateTimeSlotRequest request);
    }
}
