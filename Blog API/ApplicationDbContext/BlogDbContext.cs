using Blog_API.Modules.Users;
using Blog_API.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog_API.Modules.Blog;

namespace Blog_API.ApplicationDbContext
{
    public class BlogDbContext : IdentityDbContext<ApplicationUser , ApplicationRole , Guid>
    {

        public BlogDbContext(DbContextOptions options) : base(options)
        {
                
        }

        public DbSet<UsersEntity> Users { get; set; }

        public DbSet<BlogEntity> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Fluent Api Fro add default value data type and ...

            builder.Entity<UsersEntity>().Property(p => p.UserName)
                .HasColumnName("unige Username")
                .HasColumnType("varchar(14)")
                .HasDefaultValue("ApplicationUser");

            //Fluent Api For adding unique column
            builder.Entity<UsersEntity>().HasIndex(i => i.Email)
                .IsUnique();

           

        }
    }
}
