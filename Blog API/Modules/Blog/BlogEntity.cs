using System.ComponentModel.DataAnnotations;

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
    }
}
