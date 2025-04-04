using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adventour.Api.Models
{
    public class Country
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(2)]
        [Column("code")]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("continent_name")]
        public string ContinentName { get; set; }

        [Column("svg")]
        public string Svg { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
