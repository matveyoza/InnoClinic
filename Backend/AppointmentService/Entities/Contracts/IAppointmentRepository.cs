using Entities.Models;

namespace Entities.Contracts
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByRoleAsync(
            Guid userId,
            string role,
            CancellationToken cancellationToken);

        Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);

        Task DeleteAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
    }
}
