using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public Country Country { get; set; }
        public ICollection<Attraction> Attractions { get; set; }
    }
}
