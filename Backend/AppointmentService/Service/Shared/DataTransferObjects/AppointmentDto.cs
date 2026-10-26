using Entities.Models;

namespace Service.Shared.DataTransferObjects
{
    public record AppointmentDto
    {
        public Guid Id { get; init; }
        public string PatientName { get; init; } = string.Empty;
        public string DoctorName { get; init; } = string.Empty;
        public string DoctorSpecialty { get; init; } = string.Empty;
        public string Date { get; init; }
        public string Time { get; init; }
        public string Location { get; init; } = string.Empty;
        public AppointmentStatus Status { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public IReadOnlyList<AppointmentDocumentItemDto> Documents { get; init; } = [];
    }
}
