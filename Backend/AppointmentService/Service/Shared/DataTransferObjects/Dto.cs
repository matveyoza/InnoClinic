using Entities.Models;

namespace Service.Shared.DataTransferObjects
{
    public record Dto
    {
        public Guid Id { get; init; }
        public string PatientName { get; init; } = string.Empty;
        public string DoctorName { get; init; } = string.Empty;
        public string DoctorSpecialty { get; init; } = string.Empty;
        public DateOnly Date { get; init; }
        public TimeOnly Time { get; init; }
        public int Room { get; init; }
        public AppointmentStatus Status { get; init; }
        public DateTime CreatedAtUtc { get; init; }
        public IReadOnlyList<DocumentDto> Documents { get; init; } = [];
    }
}
