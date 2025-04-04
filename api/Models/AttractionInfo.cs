namespace Adventour.Api.Models
{
    public class AttractionInfo
    {
        public int Id { get; set; }

        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        public int AttractionInfoTypeId { get; set; }
        public AttractionInfoType AttractionInfoType { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int? DurationSeconds { get; set; }
    }
}
