using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventour.Api.Models.Database
{
    public class Attraction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [Column("short_description")]
        public string? ShortDescription { get; set; }

        public ICollection<AttractionImages> AttractionImages { get; set; }

        [Required]
        [ForeignKey("Country")]
        [Column("id_country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Column("average_rating")]
        public int? AverageRating { get; set; }

        [Column("address_one")]
        public string? AddressOne { get; set; }

        [Column("address_two")]
        public string? AddressTwo { get; set; }

        [Column("long_description")]
        public string? LongDescription { get; set; }

        public ICollection<AttractionInfo> AttractionInfos { get; set; }
    }
}
