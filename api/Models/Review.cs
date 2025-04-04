namespace Adventour.Api.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int RatingId { get; set; }
        public Rating Rating { get; set; }

        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        public Guid UserId { get; set; }
        public Person Person { get; set; }

        public string Comment { get; set; }
    }
}
