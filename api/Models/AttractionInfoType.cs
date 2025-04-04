using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class AttractionInfoType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("type_title")]
        public int TypeTitle { get; set; }
    }
}
