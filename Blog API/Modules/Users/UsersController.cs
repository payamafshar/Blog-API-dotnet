using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Modules.Users
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("Clinet2")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        public UsersController(IMapper mapper)
        {
            _mapper = mapper;   
        }
        [HttpGet]
        public IActionResult Create()
        {
            return BadRequest("hello");
        }
    }
}
