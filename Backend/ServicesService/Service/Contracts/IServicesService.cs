using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IServicesService
    {
        Task<IEnumerable<BookedTimeSegmentDto>> GetDoctorBookedSegmentsAsync(Guid doctorId, DateOnly? date = null, CancellationToken cancellationToken = default);

        Task<bool> BookTimeSegmentAsync(BookTimeSegmentDto dto, CancellationToken cancellationToken = default);
        Task<IEnumerable<DoctorProfileDto>> GetAllDoctorProfilesAsync(CancellationToken cancellationToken = default);
    }
}
