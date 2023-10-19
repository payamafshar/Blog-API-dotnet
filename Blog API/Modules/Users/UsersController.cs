using AutoMapper;
using Blog_API.CustomController;
using Blog_API.Identity;
using Blog_API.Modules.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Modules.Users
{
    //Enabling cors with another Orgin that Default Origin
    // [EnableCors("Clinet2")]
    //Disabling Authorization for this controller
    [AllowAnonymous]

    //Extending From CustomController and Implementing Route And ControllerName
    public class UsersController : CustomControllerBase
    {
        
        private readonly IUserService _usersService;
        public UsersController(
            IUserService usersService
            )
        {
            _usersService = usersService;
        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ApplicationUser>> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join('|',
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }
            var authenticationResponse = await _usersService.Register(registerDto);
           
            return Ok(authenticationResponse);
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
            var authenticationResponse = await _usersService.Login(loginDto);
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
            var authenticationResponse = await _usersService.GenerateNewAccessToken(refreshTokenDto);
            return Ok(authenticationResponse);
        }
    }
}
