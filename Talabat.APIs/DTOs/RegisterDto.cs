using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string DisplayName { get; set; }
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d])[a-zA-Z\d\S]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }


    }
}
