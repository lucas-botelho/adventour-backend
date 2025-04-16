using Adventour.Api.Models.Database;
using Adventour.Api.Responses.TimeSlots;

namespace Adventour.Api.Responses.Day
{
    public class BasicDayDetails
    {
        public int Id { get; set; }
        public int ItineraryId { get; set; }
        public int DayNumber { get; set; }
        public ICollection<BasicTimeSlotDetails> Timeslots { get; set; } = new List<BasicTimeSlotDetails>();
    }
}
