using Users.Entities.Shared;
using Users.Service.Shared.DataTransferObjects;

namespace Users.Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default);
        Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> CreateUserProfileAsync(UserProfileDto profileDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
