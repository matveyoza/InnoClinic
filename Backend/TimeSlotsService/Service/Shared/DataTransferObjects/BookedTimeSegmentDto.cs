namespace Service.Shared.DataTransferObjects
{
    public sealed record BookedTimeSegmentDto
    {
        public Guid Id { get; init; }
        public Guid DoctorId { get; init; }
        public DateOnly Date { get; init; }
        public TimeOnly StartTime { get; init; }
        public TimeOnly EndTime { get; init; }
    }
}
