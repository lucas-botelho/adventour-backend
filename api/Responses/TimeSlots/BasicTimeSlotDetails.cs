﻿using Adventour.Api.Models.Database;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Adventour.Api.Responses.Attractions;

namespace Adventour.Api.Responses.TimeSlots
{

    public class BasicTimeSlotDetails
    {
        public int Id { get; set; }
        public BasicAttractionDetails? Attraction { get; set; }
        public int DayId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
