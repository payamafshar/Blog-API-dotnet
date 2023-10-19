using AutoMapper;
using Blog_API.Identity;
using Blog_API.JwtService;
using Blog_API.Modules.Users.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Modules.Users
{
    public class UsersService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public UsersService(
              UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IMapper mapper

            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _mapper = mapper;

        }
        public async Task<AuthenticationResponse> Register(RegisterDto registerDto)
        {
            Console.WriteLine(new { registerDto });
            ApplicationUser existUserWithEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if(existUserWithEmail != null)
            {
                throw new BadHttpRequestException("User With This Email Already Exist");
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,

            };
          IdentityResult result =  await _userManager.CreateAsync(user ,registerDto.Password);
            if (result.Succeeded)
            {
               await _signInManager.SignInAsync(user , isPersistent: false  );

               AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
                return authenticationResponse;
            }
            string errorMessage = string.Join("", result.Errors.SelectMany(e => e.Description));
            throw new BadHttpRequestException(errorMessage);
        }


        public async Task<AuthenticationResponse> Login(LoginDto loginDto)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Credentials");
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
                return authenticationResponse;
            }
            
            throw new UnauthorizedAccessException("Invalid Credintials1");
        }
    }
}
