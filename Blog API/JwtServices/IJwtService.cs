using Blog_API.Identity;
using Blog_API.Modules.Users.Dtos;
using System.Security.Claims;

namespace Blog_API.JwtServices
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(ApplicationUser user);
        string GenerateRefreshToken();

        ClaimsPrincipal? GetClaimsPrincipalFromJwtToken(string? token);
    }
}
