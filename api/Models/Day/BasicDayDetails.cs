using Adventour.Api.Models.TimeSlots;

namespace Adventour.Api.Models.Day
{
    public class BasicDayDetails
    {
        public int Id { get; set; }
        public int ItineraryId { get; set; }
        public int DayNumber { get; set; }
        public ICollection<BasicTimeSlotDetails> Timeslots { get; set; } = new List<BasicTimeSlotDetails>();
    }
}
