﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Adventour.Api.Models.Database
{
    public class Day
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Itinerary")]
        [Column("id_itinerary")]
        public int ItineraryId { get; set; }
        public Itinerary Itinerary { get; set; }

        [Required]
        [Column("day_number")]
        public int DayNumber { get; set; }

        public ICollection<Timeslot> Timeslots { get; set; }
    }
}
