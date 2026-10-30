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
        public DbSet<AppointmentDocument> AppointmentDocuments => Set<AppointmentDocument>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointments");
                entity.HasKey(a => a.Id);

                entity.Property(a => a.PatientId)
                    .IsRequired();

                entity.Property(a => a.DoctorId)
                    .IsRequired();

                entity.Property(a => a.PatientName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(a => a.DoctorName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(a => a.DoctorSpecialty)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(a => a.DoctorAvatarUrl)
                    .HasMaxLength(2083)
                    .IsRequired(false);

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
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(a => a.CreatedAtUtc)
                    .IsRequired();

                entity.HasMany(a => a.Documents)
                    .WithOne(d => d.Appointment)
                    .HasForeignKey(d => d.AppointmentId);

                entity.HasIndex(a => new { a.PatientId, a.Date, a.Time })
                    .HasDatabaseName("IX_Appointments_PatientId_Date_Time");

                entity.HasIndex(a => new { a.DoctorId, a.Date, a.Time })
                    .HasDatabaseName("IX_Appointments_DoctorId_Date_Time");

                entity.HasIndex(a => new { a.Date, a.Time })
                    .HasDatabaseName("IX_Appointments_Date_Time");
            });

            modelBuilder.Entity<AppointmentDocument>(entity =>
            {
                entity.ToTable("AppointmentDocuments");
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(d => d.DataType)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(d => d.Url)
                    .HasMaxLength(2083)
                    .IsRequired(false);

                entity.HasIndex(d => d.AppointmentId)
                    .HasDatabaseName("IX_AppointmentDocuments_AppointmentId");
            });
        }
    }
}
