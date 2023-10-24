using AutoMapper;
using Blog_API.ApplicationDbContext;
using Blog_API.Identity;
using Blog_API.Modules.Likes_Comments.Dtos;
using Blog_API.Modules.Likes_Comments.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Modules.Likes_Comments
{
    public class LikesAndCommentService : ILikeAndCommentService
    {
        private readonly BlogDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public LikesAndCommentService(BlogDbContext dbContext, UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<string> CreateToggleLikeAsync(Guid blogId , string email)
        {

            var findedBlog = await _dbContext.Blogs.Include("Likes").FirstOrDefaultAsync(temp => temp.Id == blogId);
            Console.WriteLine(findedBlog.title);
            if (findedBlog == null)
            {
                return null;
            }
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            Console.WriteLine(user.Id);
             LikesEntity? existLike = user.Likes?.FirstOrDefault(temp => temp.BlogId == blogId);
            if (existLike == null)
            {
                 var likeDto = new LikeDto()
                {
                    Blog = findedBlog,
                    BlogId = findedBlog.Id,
                    User = user,
                    UserId = user.Id
                };
              var  mapperEntity = _mapper.Map<LikesEntity>(likeDto);

                var createdLike = await _dbContext.Likes.AddAsync(mapperEntity);
                await _dbContext.SaveChangesAsync();
                
                return "Blog Liked";
            }
            user.Likes.Remove(existLike);
            var updateResult = await _userManager.UpdateAsync(user);
            if(updateResult.Succeeded == true)
            {
                return "Blog Dissliked";
            }
            return "Internal Server Error";
        }
    }
}
