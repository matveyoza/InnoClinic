using Entities.Models;

namespace Entities.Contracts
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAsync(Appointment appointment, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
