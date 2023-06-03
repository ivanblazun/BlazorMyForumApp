using FirstBlazorApp.Models;
using ForumAdminPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstBlazorApp.Data
{
    public class appDbContext : DbContext
    {
        public appDbContext(DbContextOptions<appDbContext> options) : base (options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Fora> Fora { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<MainForum> MainForums { get; set; }

        public DbSet<Images> Images { get; set; }

    }
}
