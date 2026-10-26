using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AppointmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
