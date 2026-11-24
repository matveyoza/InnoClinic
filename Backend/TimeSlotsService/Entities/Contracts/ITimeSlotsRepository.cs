using Entities.Models;

namespace Entities.Contracts
{
    public interface ITimeSlotsRepository
    {
        Task<IEnumerable<BookedTimeSegment>> GetByDoctorIdAsync(Guid doctorId, DateOnly? date = null, CancellationToken cancellationToken = default);

        Task<bool> HasOverlapAsync(Guid doctorId, DateOnly date, TimeOnly startTime, TimeOnly endTime, CancellationToken cancellationToken = default);

        Task AddAsync(BookedTimeSegment segment, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
