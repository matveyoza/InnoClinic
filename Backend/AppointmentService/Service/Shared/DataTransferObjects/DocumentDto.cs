namespace Service.Shared.DataTransferObjects
{
    public record DocumentDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string? Url { get; init; }
    }
}
