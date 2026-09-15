
namespace Service.Shared.DataTransferObjects
{
    public record UserDto
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
