namespace Service.Shared
{
    public sealed record UserForAuthenticationDto
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
