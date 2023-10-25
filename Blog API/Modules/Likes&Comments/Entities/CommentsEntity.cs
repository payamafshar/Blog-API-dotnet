﻿using Blog_API.Identity;
using Blog_API.Modules.Blog;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_API.Modules.Likes_Comments.Entities
{
    public class CommentsEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Comment { get; set; }

        public Guid BlogId { get; set; }
        [ForeignKey(nameof(BlogId))]
        public BlogEntity Blog { get; set; }
        public Guid AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }
        public ICollection<RepyCommentEntity>? ReplyComment { get; set; }
    }
}
