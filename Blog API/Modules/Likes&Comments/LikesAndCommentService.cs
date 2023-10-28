using AutoMapper;
using Blog_API.ApplicationDbContext;
using Blog_API.Modules.Blog;
using Blog_API.Modules.Likes_Comments.Dtos;
using Blog_API.Modules.Likes_Comments.Entities;
using Blog_API.Modules.Likes_Comments.Execptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Modules.Likes_Comments
{
    public class LikesAndCommentService : ILikeAndCommentService
    {
        private readonly BlogDbContext _dbContext;
   
        private readonly IMapper _mapper;
        public LikesAndCommentService(BlogDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
          
            _mapper = mapper;
        }

        public async Task<CommentsEntity> CreateCommentAsync(CreateCommentDto createCommentDto,Guid blogId, string email)
        {
            var findedBlog = await _dbContext.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
           
            Console.WriteLine(findedBlog?.title);
            if (findedBlog == null)
            {
                return null;
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            var mappedComment = _mapper.Map<CommentsEntity>(createCommentDto);
            mappedComment.Blog = findedBlog;
            mappedComment.BlogId = findedBlog.Id;
            mappedComment.Author = user;
            mappedComment.AuthorId = user.Id;
            await _dbContext.Comments.AddAsync(mappedComment);
            await _dbContext.SaveChangesAsync();
            return mappedComment;
        }

        public async Task<RepyCommentEntity> CreateRepyCommentAsync(CreateReplyCommentDto createReplyCommentDto,  string email, Guid commentId)
        {
      

            var findedComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if(findedComment == null)
            {
                throw new NotFoundException("comment Not Found");
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            var mappedReplyComment = _mapper.Map<RepyCommentEntity>(createReplyCommentDto);

            mappedReplyComment.Author = user;
            mappedReplyComment.AuthorId = user.Id;
            mappedReplyComment.Comment = findedComment;
            mappedReplyComment.CommentId = findedComment.Id;

            await _dbContext.ReplyComments.AddAsync(mappedReplyComment);
            await _dbContext.SaveChangesAsync();
            return mappedReplyComment;
        }

        public async Task<string> CreateToggleLikeAsync(Guid blogId , string email)
        {

            var findedBlog = await _dbContext.Blogs.Include("Likes").FirstOrDefaultAsync(temp => temp.Id == blogId);
            Console.WriteLine(findedBlog.title);
            if (findedBlog == null)
            {
                return null;
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
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
            await _dbContext.SaveChangesAsync();
            
                return "Blog Dissliked";
     
        }

        public async Task<List<BlogEntity>> GetAllBlogsAsync()
        {
            return await _dbContext.Blogs.Include(blog => blog.Comments).ThenInclude(comment => comment.ReplyComment).ToListAsync();
        }
    }
}
