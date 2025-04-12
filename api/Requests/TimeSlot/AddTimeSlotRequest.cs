namespace Adventour.Api.Requests.TimeSlot
{
    public class AddTimeSlotRequest
    {
        public int DayId { get; set; }
        public int AttractionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
