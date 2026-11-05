namespace Service.Shared.DataTransferObjects
{
    public record AppointmentForCreationDto
    {
        public Guid PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public Guid DoctorId { get; init; }
        public string DoctorName { get; init; } = string.Empty;
        public string DoctorSpecialty { get; init; } = string.Empty;
        public string? DoctorAvatarUrl { get; init; }
        public DateOnly Date { get; init; }
        public TimeOnly Time { get; init; }
        public int Room { get; init; }
        public IReadOnlyList<AppointmentDocumentForCreationDto>? Documents { get; init; }
    }
}
