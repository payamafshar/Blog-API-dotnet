using AutoMapper;
using Blog_API.ChatHubs;
using Blog_API.CustomController;
using Blog_API.Modules.Blog.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Blog_API.Modules.Blog
{
    

   
    public class BlogController : CustomControllerBase
       
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        private readonly IHubContext<MessaginHub> _hubcontext;

        public BlogController(IBlogService blogService, IMapper mapper, IHubContext<MessaginHub> hubcontext)
        {
            _blogService = blogService;
            _mapper = mapper;
            _hubcontext = hubcontext;
        }
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BlogEntity>> Create(CreateBlogDto createBlogDto)
        {
            var blog = await _blogService.Create(createBlogDto);

            return blog;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<BlogEntity>>> GetAll()  
        {
           
            var blogs =  await _blogService.GetAll();
          
            await _hubcontext.Clients.All.SendAsync("SendMessage", "asdsad" );
            return blogs;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BlogEntity>> Get([FromRoute] Guid id)
        {
            var findedBlog = await _blogService.Get(id);
            if(findedBlog == null)
            {
                return NotFound();
            }
            return findedBlog;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<BlogEntity>> Update([FromRoute] Guid id , UpdateBlogDto updateBlogDto)
        {
            var updatedBlog = await _blogService.Update(updateBlogDto, id);

            if(updateBlogDto == null)
            {
                return NotFound();
            }
            return updatedBlog;
        }
    }
}
