using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Requests.Attraction
{
    public class AddReviewRequest
    {
        [Required]
        public string OAuthId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(250)]
        public string Review { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public IEnumerable<string> ImagesUrls { get; set; } = new List<string>();
    }
}