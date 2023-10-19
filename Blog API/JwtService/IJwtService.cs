using Blog_API.Identity;
using Blog_API.Modules.Users.Dtos;

namespace Blog_API.JwtService
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
    }
}
