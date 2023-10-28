
using AutoMapper;
using Blog_API.ApplicationDbContext;
using Blog_API.CustomController;
using Blog_API.JwtServices;
using Blog_API.Modules.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IHttpContextAccessor _httpContext;
        private readonly BlogDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public UsersController(


            IJwtService jwtService,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,
            BlogDbContext dbContext


            )
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _httpContext = httpContextAccessor;

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
            UsersEntity? existUserWithEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existUserWithEmail != null)
            {
                return BadRequest("Email Already Exists");
            }
         
            var mappedUser = _mapper.Map<UsersEntity>(registerDto);


            

            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(mappedUser);
            Console.WriteLine(authenticationResponse.Token);
            mappedUser.RefreshToken = authenticationResponse.RefreshToken;
            mappedUser.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _dbContext.Users.AddAsync(mappedUser);
            await _dbContext.SaveChangesAsync();
                //_webHostEnvironment.IsDevelopment() == true ? sign a Cookie with secure =false : sign a Cookie with Secure =true
                return Ok(authenticationResponse);
           
         
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UsersEntity>> Login(LoginDto loginDto)
        {

            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join('|',
                   ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid Credentials");
            }
        
            
           
                AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
                user.RefreshToken = authenticationResponse.RefreshToken;
                user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            //_webHostEnvironment.IsDevelopment() == true ? sign a Cookie with secure =false : sign a Cookie with Secure =true
           var updatedUser =  await _dbContext.Users.Where(u => u.Email == user.Email).ExecuteUpdateAsync(setter => setter.SetProperty(r => r.RefreshToken, authenticationResponse.RefreshToken).SetProperty(r => r.RefreshTokenExpirationDateTime, authenticationResponse.RefreshTokenExpirationDateTime));
         
                return Ok(authenticationResponse);
         
        
           
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

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid Refresh Token");
            }

            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            var updatedUser = await _dbContext.Users.Where(u => u.Email == user.Email).ExecuteUpdateAsync(setter => setter.SetProperty(r => r.RefreshToken, authenticationResponse.RefreshToken).SetProperty(r => r.RefreshTokenExpirationDateTime, authenticationResponse.RefreshTokenExpirationDateTime)); ;
            return Ok(authenticationResponse);
        }

        [HttpGet]
        [Route("loginuser")]
        [Authorize]

        public  async Task<ActionResult<UsersEntity>>  GetLoginUser()
        {
            string? Email = _httpContext.HttpContext?.User.Email();
            if(Email == null)
            {
                return Unauthorized();
            }
            var user = await _dbContext.Users.Include(u => u.Likes).ThenInclude(like => like.Blog).FirstOrDefaultAsync(item => item.Email == Email);
            return Ok(user);
        }
    }
}
