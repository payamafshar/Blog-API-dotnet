using Blog_API.Modules.Likes_Comments.Entities;
using System.ComponentModel.DataAnnotations;

namespace Blog_API.Modules.Users
{
    public class UsersEntity
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(14)]
        public string UserName { get; set; }

        public string Password { get; set; }
        [Required]
        [StringLength(35)]
        public string Email { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpirationDateTime { get; set; }

        public ICollection<LikesEntity>? Likes { get; set; }

        public ICollection<CommentsEntity>? Comments { get; set; }


    }
}
