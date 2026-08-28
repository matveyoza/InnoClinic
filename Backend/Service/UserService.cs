using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Service.Shared.DataTransferObjects;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager) =>
            _userManager = userManager;

        public async Task<IEnumerable<UserDto>> GetUsersAsync() =>
            await _userManager.Users
            .AsNoTracking()
            .Select(user => new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            })
            .ToListAsync();


        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return null;

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            };
        }

        public async Task<IdentityResult> CreateUserAsync(UserForCreationDto userForCreation)
        {
            var user = new User
            {
                FirstName = userForCreation.FirstName,
                LastName = userForCreation.LastName,
                UserName = userForCreation.UserName,
                Email = userForCreation.Email,
                NormalizedUserName = userForCreation.UserName.ToUpperInvariant(),
                NormalizedEmail = userForCreation.Email.ToUpperInvariant()
            };
            return await _userManager.CreateAsync(user, userForCreation.Password);
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return IdentityResult.Failed();

            return await _userManager.DeleteAsync(user);
        }
    }
}
