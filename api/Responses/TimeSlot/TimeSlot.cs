namespace Adventour.Api.Responses.TimeSlot
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public int AttractionId { get; set; }
        public int DayId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
