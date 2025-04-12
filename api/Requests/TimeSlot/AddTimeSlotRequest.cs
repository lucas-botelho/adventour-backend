namespace Adventour.Api.Requests.TimeSlot
{
    public class AddTimeSlotRequest
    {
        public string UserId { get; set; } = string.Empty;
        public int DayId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
