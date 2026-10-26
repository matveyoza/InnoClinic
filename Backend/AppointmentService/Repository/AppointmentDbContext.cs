using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AppointmentDocumentItem> AppointmentDocuments => Set<AppointmentDocumentItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.PatientName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(a => a.DoctorName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(a => a.DoctorSpecialty)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(a => a.Date)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(a => a.Time)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(a => a.Cabinet)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(a => a.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30);

                entity.HasMany(a => a.Documents)
                    .WithOne(d => d.Appointment)
                    .HasForeignKey(d => d.AppointmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AppointmentDocumentItem>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(d => d.Type)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(d => d.Url)
                    .HasMaxLength(2083);
            });
        }
    }
}
