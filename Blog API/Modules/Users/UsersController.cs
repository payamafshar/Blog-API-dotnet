using AutoMapper;
using Blog_API.CustomController;
using Blog_API.Identity;
using Blog_API.JwtServices;
using Blog_API.Modules.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog_API.Modules.Users
{
    //Enabling cors with another Orgin that Default Origin
    // [EnableCors("Clinet2")]
    //Disabling Authorization for this controller
    [AllowAnonymous]

    //Extending From CustomController and Implementing Route And ControllerName
    public class UsersController : CustomControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public UsersController(
              UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment

            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid == false)
            {
                string errorMessages = string.Join('|',
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessages);
            }
            ApplicationUser existUserWithEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existUserWithEmail != null)
            {
                return BadRequest("Email Already Exists");
            }
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,

            };
            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
                user.RefreshToken = authenticationResponse.RefreshToken;
                user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);
                //_webHostEnvironment.IsDevelopment() == true ? sign a Cookie with secure =false : sign a Cookie with Secure =true
                return Ok(authenticationResponse);
            }
         
            string errorMessage = string.Join("", result.Errors.SelectMany(e => e.Description));

            return BadRequest(errorMessage);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ApplicationUser>> Login(LoginDto loginDto)
        {

            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join('|',
                   ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }
            ApplicationUser user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid Credentials");
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
                user.RefreshToken = authenticationResponse.RefreshToken;
                user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
                //_webHostEnvironment.IsDevelopment() == true ? sign a Cookie with secure =false : sign a Cookie with Secure =true
                await _userManager.UpdateAsync(user);
                return Ok(authenticationResponse);
            }
            return BadRequest("Invalid Credentials");
           
        }

        [HttpPost]
        [Route("refresh_token")]
        public async Task<ActionResult> GenerateNewAccessToken(RefreshTokenDto refreshTokenDto)
        {
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join('|',
                   ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }
            if (refreshTokenDto == null)
            {
                return BadRequest("Invalid Client Request");
            }

            string? accesToken = refreshTokenDto.Token;
            string? refreshToken = refreshTokenDto.RefreshToken;

            ClaimsPrincipal? principal = _jwtService.GetClaimsPrincipalFromJwtToken(accesToken);

            if (principal == null)
            {
                return BadRequest("Invalid Jwt Token");
            }

            string? email = principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid Refresh Token");
            }

            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);
            return Ok(authenticationResponse);
        }
    }
}
