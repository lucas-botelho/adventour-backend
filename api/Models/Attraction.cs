using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Adventour.Api.Models
{
    public class Attraction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("City")]
        [Column("city_id")] // Mapeamento para a coluna correta no SQL
        public int? CityId { get; set; }
        public City City { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; }

        [Column("average_rating")]
        public int? AverageRating { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("address_one")]
        public string? AddressOne { get; set; }

        [Column("address_two")]
        public string? AddressTwo { get; set; }

        public ICollection<AttractionInfo> AttractionInfos { get; set; }
        public ICollection<AttractionImages> AttractionImages { get; set; }
    }
}
