namespace Service.Shared.DataTransferObjects
{
    public sealed record BookTimeSegmentDto
    {
        public Guid DoctorId { get; init; }
        public DateOnly Date { get; init; }
        public TimeOnly StartTime { get; init; }
    }
}
