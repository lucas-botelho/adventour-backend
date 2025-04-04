using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class ReviewImages
    {
        public int Id { get; set; }

        [ForeignKey("Review")]
        public int ReviewId { get; set; }
        public Review Review { get; set; }

        public bool IsMain { get; set; }

        [Required]
        [MaxLength(255)]
        public string PictureRef { get; set; }
    }
}
