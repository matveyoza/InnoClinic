

namespace Service.Shared.DataTransferObjects
{
    public record UserProfileDto
    {
        public string Id { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? UserName { get; init; }
        public string Role { get; set; } = string.Empty;
        public string? Specialty { get; init; }
        public int? RoomNumber { get; init; }
    }
}
