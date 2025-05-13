using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Adventour.Api.Models.Database
{
    public class Person
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("oauth_id")]
        public string? OauthId { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("name")]
        public string Name { get; set; }

        [MaxLength(25)]
        [Column("username")]
        public string? Username { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("email")]
        public string Email { get; set; }

        [Column("verified")]
        public bool Verified { get; set; }

        [Column("photo_url")]
        public string? PhotoUrl { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Itinerary> Itineraries { get; set; }
    }
}
