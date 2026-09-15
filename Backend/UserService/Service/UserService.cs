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

        public async Task<IdentityResult> CreateUserProfileAsync(UserProfileDto profileDto)
        {
            var user = new User
            {
                Id = profileDto.Id,
                Email = profileDto.Email,
                UserName = profileDto.UserName ?? profileDto.Email,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName
            };

            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return IdentityResult.Failed();

            return await _userManager.DeleteAsync(user);
        }

        public async Task<UserForAuthDto?> GetUserForAuthByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return null;

            return new UserForAuthDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                PasswordHash = user.PasswordHash ?? string.Empty
            };
        }
    }
}
