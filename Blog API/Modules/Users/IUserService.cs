using Blog_API.Identity;
using Blog_API.Modules.Users.Dtos;

namespace Blog_API.Modules.Users
{
    public interface IUserService
    {
        Task<AuthenticationResponse> Register(RegisterDto registerDto);
        Task<AuthenticationResponse> Login(LoginDto loginDto);
    }
}
