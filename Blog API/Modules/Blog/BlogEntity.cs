using Blog_API.Modules.Likes_Comments.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_API.Modules.Blog
{
    public class BlogEntity
    {
        [Key]
        public Guid Id { get;set; }
        [StringLength(40)]
        public string title { get;set; }


        public string img { get; set; }


        public string text { get; set; }
      
        public ICollection<LikesEntity>? Likes { get; set; }
    }
}
