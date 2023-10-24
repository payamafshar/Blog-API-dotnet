using AutoMapper;
using Blog_API.ApplicationDbContext;
using Blog_API.EexceptionMiddleware;
using Blog_API.Modules.Blog.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Modules.Blog
{
    public class BlogService : IBlogService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IMapper _mapper;
        public BlogService(BlogDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BlogEntity> Create(CreateBlogDto createBlogDto)
        {

            var blog = _mapper.Map<BlogEntity>(createBlogDto);
            await _dbContext.Blogs.AddAsync(blog);
            await _dbContext.SaveChangesAsync();
            return blog;
        }

        public Task<BlogEntity> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogEntity> Get(Guid id)
        {
            var existsBlog = await _dbContext.Blogs.FirstOrDefaultAsync(temp => temp.Id == id);
            if (existsBlog == null)
            {
                return null;
            }
            return existsBlog;
        }

        public async Task<List<BlogEntity>> GetAll()
        {
           
            var blogs = await _dbContext.Blogs.ToListAsync();
            
            return blogs;
        }

        public async Task<BlogEntity> Update(UpdateBlogDto updateBlogDto,Guid id )
        {
            var existsBlog = await _dbContext.Blogs.FirstOrDefaultAsync(temp => temp.Id == id);
            if (existsBlog == null)
            {
                return null;
            }

            existsBlog.title = updateBlogDto?.title ?? existsBlog.title;
            existsBlog.text = updateBlogDto?.text ?? existsBlog.text;
            existsBlog.img = updateBlogDto?.img ?? existsBlog.img;

            await _dbContext.SaveChangesAsync();
            return existsBlog;
        }
    }
}
