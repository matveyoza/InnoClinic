namespace Service.Shared.DataTransferObjects
{
    public sealed record UserForAuthenticationDto
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
