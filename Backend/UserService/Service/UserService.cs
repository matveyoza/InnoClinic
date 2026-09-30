using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Service.Shared.DataTransferObjects;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;

        public UserService(UserManager<User> userManager, IMapper mapper, ILogger logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken) =>
            await _userManager.Users
                .AsNoTracking()
                .Select(user => new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty
                })
                .ToListAsync(cancellationToken);


        public async Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString(), cancellationToken);

            if (user is null)
                return null;

            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty
            };
        }

        public async Task<bool> CreateUserProfileAsync(UserProfileDto profileDto)
        {
            var user = _mapper.Map<User>(profileDto);

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogInformation($"Failed profile creation for ID {user.Id}.");
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return false;

            return true;
        }
    }
}
