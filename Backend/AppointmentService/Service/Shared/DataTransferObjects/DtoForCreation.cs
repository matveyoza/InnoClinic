namespace Service.Shared.DataTransferObjects
{
    public record AppointmentForCreationDto
    {
        public string PatientName { get; init; } = string.Empty;
        public string DoctorName { get; init; } = string.Empty;
        public string DoctorSpecialty { get; init; } = string.Empty;
        public DateOnly Date { get; init; }
        public TimeOnly Time { get; init; }
        public int Room { get; init; }
        public IReadOnlyList<DocumentForCreationDto>? Documents { get; init; }
    }
}
