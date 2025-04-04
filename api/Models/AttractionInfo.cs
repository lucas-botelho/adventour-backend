using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class AttractionInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Attraction")]
        [Column("attraction_id")]
        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; }

        [ForeignKey("AttractionInfoType")]
        [Column("attraction_info_type_id")]
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
