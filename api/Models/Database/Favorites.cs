using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models.Database
{
    public class Favorites
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Attraction")]
        [Column("id_attraction")]
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        [ForeignKey("Person")]
        [Column("id_user")]
        public Guid UserId { get; set; }
        public Person Person { get; set; }
    }
}
