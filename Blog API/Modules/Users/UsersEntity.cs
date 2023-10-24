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
        [StringLength(14)]
        public string Email { get; set; }

       
    }
}
