using Microsoft.AspNetCore.Identity;

namespace Blog_API.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        //if u want add Additional PropertyFor User Add here
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpirationDateTime { get; set; }

    }
}
