using InnoClinic.Shared.Constants;

namespace Service.Shared.DataTransferObjects
{
    public sealed record UserForRegistrationDto
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? UserName { get; init; }
        public string Role { get; init; } = AppRoles.Patient;
        public string? Specialty { get; init; }
        public int? RoomNumber { get; init; }
    }
}
