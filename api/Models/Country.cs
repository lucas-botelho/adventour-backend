using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContinentName { get; set; }

        public string Svg { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
