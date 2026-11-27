using Entities.Models;

namespace Entities.Contracts
{
    public interface IServicesRepository
    {
        Task<IEnumerable<BookedTimeSegment>> GetByDoctorIdAsync(Guid doctorId, DateOnly? date = null, CancellationToken cancellationToken = default);

        Task<bool> HasOverlapAsync(Guid doctorId, DateOnly date, TimeOnly startTime, CancellationToken cancellationToken = default);

        void Add(BookedTimeSegment segment);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<DoctorProfile>> GetAllDoctorProfilesAsync(CancellationToken cancellationToken = default);
    }
}
