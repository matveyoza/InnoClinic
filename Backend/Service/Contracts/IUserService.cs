using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<IdentityResult> CreateUserAsync(UserForCreationDto userForCreation);
        Task<IdentityResult> DeleteUserAsync(string id);
    }
}
