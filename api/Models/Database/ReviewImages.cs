using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models.Database
{
    [Table("Review_Images")]
    public class ReviewImages
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Review")]
        [Column("id_review")]
        public int ReviewId { get; set; }
        public Review Review { get; set; }

        [Column("is_main")]
        public bool IsMain { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("picture_ref")]
        public string PictureRef { get; set; }
    }
}
