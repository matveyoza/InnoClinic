using Appointments.Entities.Models;

namespace Appointments.Entities.Contracts
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByRoleAsync(Guid userId, string role, CancellationToken cancellationToken);

        Task<Appointment?> GetAppointmentByIdAsync(Guid id, CancellationToken cancellationToken);

        Task CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);

        Task DeleteAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
    }
}
