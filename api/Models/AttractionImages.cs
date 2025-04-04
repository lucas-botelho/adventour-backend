using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class AttractionImages
    {
        public int Id { get; set; }

        [ForeignKey("Attraction")]
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        public bool IsMain { get; set; }

        [Required]
        [MaxLength(255)]
        public string PictureRef { get; set; }
    }
}
