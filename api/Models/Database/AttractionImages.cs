using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models.Database
{
    [Table("Attraction_Images")]
    public class AttractionImages
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("is_main")]
        public bool IsMain { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("picture_ref")]
        public string PictureRef { get; set; }

        [ForeignKey("Attraction")]
        [Column("id_attraction")]
        public int AttractionId { get; set; }
        //public Attraction Attraction { get; set; }

    }
}
