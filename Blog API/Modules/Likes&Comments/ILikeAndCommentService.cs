using Blog_API.Modules.Likes_Comments.Entities;

namespace Blog_API.Modules.Likes_Comments
{
    public interface ILikeAndCommentService
    {
        Task<string> CreateToggleLikeAsync(Guid blogId , string email);
    }
}
