using AutoMapper;
using Blog_API.Identity;
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
        private readonly IMapper _mapper;
        public UsersService(
              UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper

            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;

        }
        public async Task<ApplicationUser> Register(RegisterDto registerDto)
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
            Console.WriteLine(new { result });
            if (result.Succeeded)
            {
               await _signInManager.SignInAsync(user , isPersistent: false  );
                return user;
            }
            string errorMessage = string.Join("", result.Errors.SelectMany(e => e.Description));
            throw new BadHttpRequestException(errorMessage);
        }

       
    }
}
