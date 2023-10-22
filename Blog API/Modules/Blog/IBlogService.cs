using Blog_API.Modules.Blog.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Modules.Blog
{
    public interface IBlogService
    {
        Task<BlogEntity> Create(CreateBlogDto blogDto);

        Task<BlogEntity> Update(UpdateBlogDto updateBlogDto, Guid id);

        Task<BlogEntity> Delete(int id);

        Task<List<BlogEntity>> GetAll();

        Task<BlogEntity> Get(Guid id);
    }
}
