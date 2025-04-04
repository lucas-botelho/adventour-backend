using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class Favorites
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Attraction")]
        [Column("attraction_id")]
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        [ForeignKey("Person")]
        [Column("user_id")]
        public Guid UserId { get; set; }
        public Person Person { get; set; }
    }
}
