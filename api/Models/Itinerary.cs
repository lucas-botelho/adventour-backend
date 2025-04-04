using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class Itinerary
    {
        public int Id { get; set; }

        [ForeignKey("Person")]
        public Guid UserId { get; set; }
        public Person Person { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public ICollection<Day> Days { get; set; }
    }
}