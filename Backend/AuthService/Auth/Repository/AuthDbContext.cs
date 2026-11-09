using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AuthDbContext : IdentityDbContext<User>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "6cb8d370-318b-453b-91f8-97f41ffbcbe3",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "6cb8d370-318b-453b-91f8-97f41ffbcbe3"
                },
                new IdentityRole
                {
                    Id = "0456ca68-6e5f-494f-9874-224c2577ad03",
                    Name = "Doctor",
                    NormalizedName = "DOCTOR",
                    ConcurrencyStamp = "0456ca68-6e5f-494f-9874-224c2577ad03"
                },
                new IdentityRole
                {
                    Id = "d4d4bfcb-eaa1-49f7-92cc-e27a9be9fb7e",
                    Name = "Patient",
                    NormalizedName = "PATIENT",
                    ConcurrencyStamp = "d4d4bfcb-eaa1-49f7-92cc-e27a9be9fb7e"
                }
            );
        }
    }
}
