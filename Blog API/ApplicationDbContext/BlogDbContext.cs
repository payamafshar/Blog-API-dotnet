using Blog_API.Modules.Users;
using Blog_API.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.ApplicationDbContext
{
    public class BlogDbContext : IdentityDbContext<ApplicationUser , ApplicationRole , Guid>
    {

        public BlogDbContext(DbContextOptions options) : base(options)
        {
                
        }

        public DbSet<UsersEntity> Users { get; set; }
    }
}
