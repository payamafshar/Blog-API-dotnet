using AutoMapper;
using Blog_API.CustomController;
using Blog_API.Identity;
using Blog_API.Modules.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        public async Task<ActionResult<ApplicationUser>> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join('|',
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }
            ApplicationUser user = await _usersService.Register(registerDto);
            Console.WriteLine(user);
           
            return Ok(user);
        }
    }
}
