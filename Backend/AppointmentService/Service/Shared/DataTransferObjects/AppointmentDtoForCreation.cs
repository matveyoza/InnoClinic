namespace Service.Shared.DataTransferObjects
{
    public record CreateAppointmentDto
    {
        public string PatientName { get; init; } = string.Empty;
        public string DoctorName { get; init; } = string.Empty;
        public string DoctorSpecialty { get; init; } = string.Empty;
        public string Date { get; init; }
        public string Time { get; init; }
        public string Location { get; init; } = string.Empty;
        public IReadOnlyList<AppointmentDocumentItemForCreationDto>? Documents { get; init; }
    }
}
