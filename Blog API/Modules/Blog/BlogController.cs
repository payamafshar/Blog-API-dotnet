using AutoMapper;
using Blog_API.CustomController;
using Blog_API.Modules.Blog.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Modules.Blog
{
    

   
    public class BlogController : CustomControllerBase
       
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;

        public BlogController(IBlogService blogService, IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;
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
            return await _blogService.GetAll();
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
