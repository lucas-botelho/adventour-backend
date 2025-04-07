using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class Rating
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("value")]
        public int Value { get; set; }
    }
}
