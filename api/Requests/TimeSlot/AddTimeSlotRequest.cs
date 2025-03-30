namespace Adventour.Api.Requests.TimeSlot
{
    public class AddTimeSlotRequest
    {
        public int AttractionId { get; set; }
        public int DayId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
