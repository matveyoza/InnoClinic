using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default);
        Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> CreateUserProfileAsync(UserProfileDto profileDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
