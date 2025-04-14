namespace Adventour.Api.Requests.TimeSlot
{
    public class UpdateTimeSlotRequest
    {
        public int TimeSlotId { get; set; }
        public int DayId { get; set; }
        public int AttractionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
