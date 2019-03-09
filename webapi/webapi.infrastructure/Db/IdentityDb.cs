using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace webapi.infrastructure.Db
{
    public class IdentityDb: IdentityDbContext<IdentityUser> 
    {
        public IdentityDb(DbContextOptions<IdentityDb> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<IdentityUser>().Property(u => u.UserName).HasMaxLength(256);
            // modelBuilder.Entity<IdentityUser>().Property(u => u.Email).HasMaxLength(256);
            // modelBuilder.Entity<IdentityUser>().Property(u => u.Id).HasMaxLength(36);
            // modelBuilder.Entity<IdentityRole>().Property(u => u.Name).HasMaxLength(256);
            // modelBuilder.Entity<IdentityRole>().Property(u => u.Id).HasMaxLength(36);
            // modelBuilder.Entity<IdentityUserLogin<string>>().Property(u => u.UserId).HasMaxLength(36);
            // modelBuilder.Entity<IdentityUserLogin<string>>().Property(u => u.ProviderDisplayName).HasMaxLength(256);
            // modelBuilder.Entity<IdentityUserLogin<string>>().Property(u => u.ProviderKey).HasMaxLength(256);
            // modelBuilder.Entity<IdentityUserLogin<string>>().Property(u => u.LoginProvider).HasMaxLength(256);
            // modelBuilder.Entity<IdentityUserToken<string>>().Property(u => u.UserId).HasMaxLength(36);
            // modelBuilder.Entity<IdentityUserToken<string>>().Property(u => u.LoginProvider).HasMaxLength(256);
            // modelBuilder.Entity<IdentityUserToken<string>>().Property(u => u.Name).HasMaxLength(256);
        }
    }
}