using Entities.Models;

namespace Entities.Contracts
{
    public interface IAppointmentRepository
    {
        Task<bool> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default);
        Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId, CancellationToken cancellationToken = default);
    }
}
