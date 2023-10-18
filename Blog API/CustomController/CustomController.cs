using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.CustomController
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
    }
}
