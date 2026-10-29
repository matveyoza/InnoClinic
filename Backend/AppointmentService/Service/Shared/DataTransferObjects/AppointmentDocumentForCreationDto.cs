namespace Service.Shared.DataTransferObjects
{
    public record AppointmentDocumentForCreationDto
    {
        public string Name { get; init; } = string.Empty;
        public string DataType { get; init; } = string.Empty;
        public string? Url { get; init; }
    }
}
