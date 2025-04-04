using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class Attraction
    {
        public int Id { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int? AverageRating { get; set; }
        public string Description { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }

        public ICollection<AttractionInfo> AttractionInfos { get; set; }
        public ICollection<AttractionImages> AttractionImages { get; set; }
    }
}
