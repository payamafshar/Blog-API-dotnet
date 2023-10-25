using Blog_API.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_API.Modules.Likes_Comments.Entities
{
    public class RepyCommentEntity
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public Guid CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public CommentsEntity Comment { get; set; }

        public Guid AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }
    }
}
