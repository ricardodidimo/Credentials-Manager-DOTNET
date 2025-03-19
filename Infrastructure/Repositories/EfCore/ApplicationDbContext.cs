using Core.Category;
using Core.Credentials;
using Core.User;
using Core.Vault;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Repositories.EfCore
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }
        public required DbSet<Vault> Vaults { get; set; }
        public required DbSet<Category> Categories { get; set; }
        public required DbSet<Credentials> Credentials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasKey(u => u.ID);
            builder.Entity<Vault>().HasKey(u => u.ID);
            builder.Entity<Credentials>().HasKey(u => u.ID);
            builder.Entity<Category>().HasKey(u => u.ID);
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
