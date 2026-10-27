using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAppointmentService
    {
        Task<Dto> CreateAsync(AppointmentForCreationDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Dto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Dto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
