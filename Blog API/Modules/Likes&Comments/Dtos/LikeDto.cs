using Blog_API.Identity;
using Blog_API.Modules.Blog;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_API.Modules.Likes_Comments.Dtos
{
    public class LikeDto
    {
        public Guid? BlogId { get; set; }

        public BlogEntity? Blog { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
