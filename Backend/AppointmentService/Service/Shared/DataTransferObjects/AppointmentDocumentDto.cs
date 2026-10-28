namespace Service.Shared.DataTransferObjects
{
    public record AppointmentDocumentDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string DataType { get; init; } = string.Empty;
        public string? Url { get; init; }
    }
}
