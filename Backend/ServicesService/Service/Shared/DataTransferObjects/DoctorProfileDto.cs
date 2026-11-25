namespace Service.Shared.DataTransferObjects
{
    public sealed record DoctorProfileDto
    {
        public Guid Id { get; init; }
        public string UserId { get; init; } = string.Empty;
        public string Specialty { get; init; } = string.Empty;
        public int RoomNumber { get; init; }
        public int Duration { get; init; }
    }
}
