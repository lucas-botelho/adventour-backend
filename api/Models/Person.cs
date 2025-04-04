using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models
{
    public class Person
    {
        public Guid Id { get; set; }

        public string OauthId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(25)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        public bool Verified { get; set; }
        public string PhotoUrl { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Itinerary> Itineraries { get; set; }
    }
}
