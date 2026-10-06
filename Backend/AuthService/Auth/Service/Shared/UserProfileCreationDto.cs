namespace Service.Shared
{
    public sealed record UserProfileCreationDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? UserName { get; init; }
    }
}
