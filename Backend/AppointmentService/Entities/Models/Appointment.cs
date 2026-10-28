namespace Entities.Models
{
    public sealed class Appointment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string PatientName { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string DoctorSpecialty { get; set; } = string.Empty;

        public string? DoctorAvatarUrl { get; set; }

        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }

        public int Room { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Upcoming;

        public ICollection<AppointmentDocument> Documents { get; set; } = new List<AppointmentDocument>();

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
