using Blog_API.Identity;
using Blog_API.Modules.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Modules.Users
{
    public interface IUserService
    {
        Task<AuthenticationResponse> Register(RegisterDto registerDto);
        Task<AuthenticationResponse> Login(LoginDto loginDto);

        Task<ActionResult<AuthenticationResponse>> GenerateNewAccessToken(RefreshTokenDto refreshTokenDto);
    }
}
