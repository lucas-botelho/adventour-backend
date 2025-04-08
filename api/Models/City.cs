using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Adventour.Api.Models
{
    public class City
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Country")]
        [Column("id_country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; }

        public ICollection<Attraction> Attractions { get; set; }
    }
}
