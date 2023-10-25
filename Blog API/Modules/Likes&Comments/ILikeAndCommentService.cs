using Blog_API.Modules.Blog;
using Blog_API.Modules.Likes_Comments.Dtos;
using Blog_API.Modules.Likes_Comments.Entities;

namespace Blog_API.Modules.Likes_Comments
{
    public interface ILikeAndCommentService
    {
        Task<string> CreateToggleLikeAsync(Guid blogId , string email);
        Task<CommentsEntity> CreateCommentAsync(CreateCommentDto createCommentDto,Guid blogId, string email);
        Task<RepyCommentEntity> CreateRepyCommentAsync(CreateReplyCommentDto createReplyCommentDto ,string email , Guid commentId);

        Task<List<BlogEntity>> GetAllBlogsAsync();
    }
}
