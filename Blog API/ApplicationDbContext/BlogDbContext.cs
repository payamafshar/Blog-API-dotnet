using Blog_API.Modules.Users;
using Microsoft.EntityFrameworkCore;
using Blog_API.Modules.Blog;
using Blog_API.Modules.Likes_Comments.Entities;


namespace Blog_API.ApplicationDbContext
{
    public class BlogDbContext : DbContext
    {

        public BlogDbContext(DbContextOptions options) : base(options)
        {
                
        }

        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<BlogEntity> Blogs { get; set; }
        public DbSet<LikesEntity> Likes { get; set; }
        public DbSet<CommentsEntity> Comments { get; set; }
        public DbSet<RepyCommentEntity> ReplyComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Fluent Api Fro add default value data type and ...

            builder.Entity<UsersEntity>().Property(p => p.UserName)
                .HasColumnName("Username")
                .HasColumnType("varchar(35)")
                .HasDefaultValue("ApplicationUser");

            //Fluent Api For adding unique column
            builder.Entity<UsersEntity>().HasIndex(i => i.Email)
                .IsUnique();

           

        }
    }
}
