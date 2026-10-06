using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Service.Shared.DataTransferObjects;
using Entities.Shared;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, IMapper mapper, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id.ToString(), cancellationToken);

            if (user is null)
            {
                return Result.Failure<UserDto>(new Error("User.NotFound", $"User with ID {id} was not found."));
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Result.Success(userDto);
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
