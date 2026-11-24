using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ITimeSlotsService
    {
        Task<IEnumerable<BookedTimeSegmentDto>> GetDoctorBookedSegmentsAsync(Guid doctorId, DateOnly? date = null, CancellationToken cancellationToken = default);

        Task<BookedTimeSegmentDto> BookTimeSegmentAsync(BookTimeSegmentDto dto, CancellationToken cancellationToken = default);
    }
}
