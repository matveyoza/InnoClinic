namespace Entities.Models
{
    public class Appointment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string PatientName { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string DoctorSpecialty { get; set; } = string.Empty;

        public string? DoctorAvatarUrl { get; set; }

        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;

        public string Cabinet { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Upcoming;


        public ICollection<AppointmentDocumentItem> Documents { get; set; } = new List<AppointmentDocumentItem>();

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
