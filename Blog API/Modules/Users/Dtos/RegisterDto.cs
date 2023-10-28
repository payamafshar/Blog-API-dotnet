using System.ComponentModel.DataAnnotations;

namespace Blog_API.Modules.Users.Dtos
{
    public class RegisterDto : IValidatableObject
    {

        public string? UserName { get; set; } = string.Empty;
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        //[Required]
        //[Compare("Password", ErrorMessage = "Password do not match")]
        //public string ConfirmPassword { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;

        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(UserName == string.Empty && FirstName == string.Empty)
            {
                yield return new ValidationResult("Either First Name Or UsersName Must Be supplied");
            }
            
            // Other validations can be applied with yield return beacuse of IEnumerable interface
        }
    }
}
