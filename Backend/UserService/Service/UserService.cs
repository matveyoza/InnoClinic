using AutoMapper;
using Entities.Models;
using Entities.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Service.Shared.Constants;
using Service.Shared.DataTransferObjects;

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

        public async Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id.ToString(), cancellationToken);

            if (user is null)
                return null;

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetDoctorsAsync(CancellationToken cancellationToken)
        {
            var doctorsInRole = await _userManager.GetUsersInRoleAsync(AppRoles.Doctor);
            var doctorIds = doctorsInRole.Select(d => d.Id).ToList();

            var doctorsWithProfiles = await _userManager.Users
                .Include(u => u.DoctorProfile)
                .Where(u => doctorIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<UserDto>>(doctorsWithProfiles);
        }

        public async Task<bool> CreateUserProfileAsync(UserProfileDto profileDto)
        {
            var user = _mapper.Map<User>(profileDto);

            if (!string.IsNullOrWhiteSpace(profileDto.Specialty))
            {
                user.DoctorProfile = new DoctorProfile
                {
                    Specialty = profileDto.Specialty,
                    RoomNumber = profileDto.RoomNumber ?? null
                };
            }

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                _logger.LogError("Failed profile creation for ID {UserId}. Errors: {Errors}", user.Id, errors);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(profileDto.Role))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, profileDto.Role);
                if (!roleResult.Succeeded)
                {
                    _logger.LogWarning("Profile created, but failed to add role {Role} in UserService.", profileDto.Role);
                }
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
