using Adventour.Api.Models.TimeSlots;
using Adventour.Api.Requests.TimeSlot;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ITimeSlotRepository
    {
        BasicTimeSlotDetails? AddTimeSlot(AddTimeSlotRequest request);
        bool RemoveTimeSlot(int idTimeSlot);
        BasicTimeSlotDetails UpdateTimeSlot(UpdateTimeSlotRequest request);
    }
}
