using System.ComponentModel.DataAnnotations;

namespace Blog_API.Modules.Blog.Dtos
{
    public class CreateBlogDto 
    {
        public string title { get; set; }

        public string img { get; set; }

        public string text { get; set; }
    }
}
