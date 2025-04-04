namespace Adventour.Api.Models
{
    public class Favorites
    {
        public int Id { get; set; }

        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        public Guid UserId { get; set; }
        public Person Person { get; set; }
    }
}
