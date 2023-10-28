using Blog_API.Modules.Users;
using Blog_API.Modules.Users.Dtos;
using System.Security.Claims;

namespace Blog_API.JwtServices
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(UsersEntity user);
        string GenerateRefreshToken();

        ClaimsPrincipal? GetClaimsPrincipalFromJwtToken(string? token);
    }
}
