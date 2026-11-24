using Entities.Models;
using Microsoft.EntityFrameworkCore;
namespace Schedule.Repository;

public class ScheduleDbContext : DbContext
{
    public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
    {
    }

    public DbSet<BookedTimeSegment> BookedTimeSegments => Set<BookedTimeSegment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookedTimeSegment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.DoctorId, e.Date });
        });
    }
}