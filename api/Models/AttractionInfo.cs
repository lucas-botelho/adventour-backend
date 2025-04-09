using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    [Table("Attraction_Info")]
    public class AttractionInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Attraction")]
        [Column("id_attraction")]
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        [ForeignKey("AttractionInfoType")]
        [Column("id_attraction_info_type")]
        public int AttractionInfoTypeId { get; set; }
        public AttractionInfoType AttractionInfoType { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("duration_seconds")]
        public int? DurationSeconds { get; set; }
    }
}
