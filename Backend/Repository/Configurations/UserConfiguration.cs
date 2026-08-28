using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;

namespace Repository.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var user = new User
            {
                Id = "a18be9c0-aa65-4af8-bd17-000000000001",
                UserName = "Ilya",
                NormalizedUserName = "ILYA",
                Email = "ilya.peshkur@innoclinic.com",
                NormalizedEmail = "ILYA.PESHKUR@INNOCLINIC.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEG4vA6S3Y2k6K9z1V6+K5W4lQ==",

                SecurityStamp = "c9d8e7f6-a5b4-3c2d-1e0f-9a8b7c6d5e4f",
                ConcurrencyStamp = "e1f2a3b4-c5d6-7e8f-9a0b-1c2d3e4f5a6b"
            };
            builder.HasData(user);
        }
    }
}
