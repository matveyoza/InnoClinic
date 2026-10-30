using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAppointmentsAsync(
            Guid userId,
            string role,
            CancellationToken cancellationToken);

        Task<AppointmentDto> CreateAppointmentAsync(
            Guid currentUserId,
            string role,
            string currentUserName,
            AppointmentForCreationDto request,
            CancellationToken cancellationToken);
    }
}
