using Adventour.Api.Requests.TimeSlot;

namespace Adventour.Api.Services.TimeSlot
{
    public interface ITimeSlotService
    {
        int AddTimeSlot(AddTimeSlotRequest request);
        void DeleteTimeSlot(int timeSlotId);
        //List<Adventour.Api.Responses.TimeSlot.TimeSlot> GetTimeSlotsByDayId(int dayId);
    }
}
