using Microsoft.AspNetCore.Identity;

namespace Blog_API.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        //if u want add Additional PropertyFor User Add here
        public string? ExtraProperty { get; set; }
    }
}
