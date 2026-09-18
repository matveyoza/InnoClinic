namespace Service.Shared
{
    public record UserForAuthenticationDto
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
