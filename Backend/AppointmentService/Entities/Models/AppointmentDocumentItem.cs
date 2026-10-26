namespace Entities.Models
{
    public class AppointmentDocumentItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string? Url { get; set; }

        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
    }
}
