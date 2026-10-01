using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Entities.Models;

namespace Repository
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.Ignore(u => u.PasswordHash);
                b.Ignore(u => u.SecurityStamp);
                b.Ignore(u => u.TwoFactorEnabled);
                b.Ignore(u => u.LockoutEnd);
                b.Ignore(u => u.LockoutEnabled);
                b.Ignore(u => u.AccessFailedCount);
            });
        }
    }
}
