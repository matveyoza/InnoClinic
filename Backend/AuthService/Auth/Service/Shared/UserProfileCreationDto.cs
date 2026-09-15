namespace Service.Shared
{
    public record UserProfileCreationDto
    {
        public string Id { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? UserName { get; init; }
    }
}
