using Blog_API.Identity;
using Blog_API.Modules.Blog;
using Blog_API.Modules.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_API.Modules.Likes_Comments.Entities
{
    public class LikesEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? BlogId { get; set; }
        [ForeignKey(nameof(BlogId))]
     
        public BlogEntity? Blog { get; set; }
        
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        
        public ApplicationUser? User { get; set;}
    }
}
