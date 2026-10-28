using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AppointmentDocument> AppointmentDocuments => Set<AppointmentDocument>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.PatientName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(a => a.DoctorName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(a => a.DoctorSpecialty)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(a => a.Date)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(a => a.Time)
                    .IsRequired()
                    .HasColumnType("time");

                entity.Property(a => a.Room)
                    .IsRequired();

                entity.Property(a => a.Status)
                    .HasConversion<string>()
                    .HasMaxLength(10);

                entity.HasMany(a => a.Documents)
                    .WithOne(d => d.Appointment)
                    .HasForeignKey(d => d.AppointmentId);
            });

            modelBuilder.Entity<AppointmentDocument>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(d => d.DataType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(d => d.Url)
                    .HasMaxLength(2083);
            });
        }
    }
}
