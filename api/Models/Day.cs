using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class Day
    {
        public int Id { get; set; }

        [ForeignKey("Itinerary")]
        public int ItineraryId { get; set; }
        public Itinerary Itinerary { get; set; }

        [Required]
        public int DayNumber { get; set; }

        public ICollection<Timeslot> Timeslots { get; set; }
    }
}