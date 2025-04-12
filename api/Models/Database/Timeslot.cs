using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models.Database
{
    public class Timeslot
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Attraction")]
        [Column("id_attraction")]
        public int? AttractionId { get; set; }
        public Attraction? Attraction { get; set; }

        [ForeignKey("Day")]
        [Column("id_day")]
        public int DayId { get; set; }
        public Day Day { get; set; }

        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }
    }
}
