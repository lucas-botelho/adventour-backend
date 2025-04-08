using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class Review
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Rating")]
        [Column("id_rating")]
        public int RatingId { get; set; }
        public Rating Rating { get; set; }

        [ForeignKey("Attraction")]
        [Column("id_attraction")]
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        [ForeignKey("Person")]
        [Column("id_user")]
        public Guid UserId { get; set; }
        public Person Person { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }
    }
}
