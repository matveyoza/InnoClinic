using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<IdentityResult> CreateUserProfileAsync(UserProfileDto profileDto);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<UserForAuthDto?> GetUserForAuthByEmailAsync(string email);
    }
}
