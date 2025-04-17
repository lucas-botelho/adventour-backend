using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models.Database
{
    public class Rating
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("rating")]
        public int Value { get; set; }
    }
}
