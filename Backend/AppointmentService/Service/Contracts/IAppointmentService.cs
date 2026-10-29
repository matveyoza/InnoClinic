using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAppointmentService
    {
        Task<bool> CreateAppointmentAsync(AppointmentForCreationDto appointmentForCreationDto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default);
        Task<AppointmentDto?> GetAppointmentByIdAsync(Guid appointmentId, CancellationToken cancellationToken = default);
    }
}
