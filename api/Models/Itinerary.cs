using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Adventour.Api.Models
{
    public class Itinerary
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Person")]
        [Column("user_id")]
        public Guid UserId { get; set; }
        public Person Person { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public ICollection<Day> Days { get; set; }
    }
}
