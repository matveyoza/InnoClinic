namespace Service.Shared
{
    public record RegisterDto
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? UserName { get; init; }
    }
}
