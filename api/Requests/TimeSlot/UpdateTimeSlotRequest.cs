namespace Adventour.Api.Requests.TimeSlot
{
    public class UpdateTimeSlotRequest
    {
        public int TimeSlotId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int DayId { get; set; }
        public int AttractionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
