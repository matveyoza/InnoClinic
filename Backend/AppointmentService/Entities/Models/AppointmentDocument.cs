namespace Entities.Models
{
    public sealed class AppointmentDocument
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string? Url { get; set; }

        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
    }
}
