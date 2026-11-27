using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ServicesDbContext : DbContext
    {
        public ServicesDbContext(DbContextOptions<ServicesDbContext> options) : base(options)
        {
        }

        public DbSet<BookedTimeSegment> BookedTimeSegments => Set<BookedTimeSegment>();
        public DbSet<DoctorProfile> DoctorProfiles => Set<DoctorProfile>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookedTimeSegment>(entity =>
            {
                entity.HasIndex(e => new { e.DoctorId, e.Date });
            });

            modelBuilder.Entity<DoctorProfile>(entity =>
            {
                entity.Property(e => e.Specialty).IsRequired().HasMaxLength(100);
            });
        }
    }
}