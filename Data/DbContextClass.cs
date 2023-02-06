using Microsoft.EntityFrameworkCore;
using role_management_user.Models;

namespace role_management_user.Data
{
    public class DbContextClass: DbContext {
        protected readonly IConfiguration Configuration;
        public DbContextClass(IConfiguration configuration) {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseNpgsql(Configuration.GetConnectionString("ConnectionString"));
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<SubMenu> SubMenus { get; set; }
        public DbSet<UserMenu> UserMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}