using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models.Authentication
{
    public class UserUpdateRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(25, ErrorMessage = "Username must be at most 25 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Username must contain only letters.")]
        public string UserName{ get; set; }
        public string PublicUrl { get; set; }
    }
}