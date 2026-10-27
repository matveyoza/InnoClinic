namespace Service.Shared.DataTransferObjects
{
    public record DocumentForCreationDto
    {
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string? Url { get; init; }
    }
}
