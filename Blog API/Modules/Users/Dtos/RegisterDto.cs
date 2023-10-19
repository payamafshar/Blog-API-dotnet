using System.ComponentModel.DataAnnotations;

namespace Blog_API.Modules.Users.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
