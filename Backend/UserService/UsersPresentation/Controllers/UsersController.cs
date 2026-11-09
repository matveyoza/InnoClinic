using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared.Constants;
using Service.Shared.DataTransferObjects;
using System.Security.Claims;

namespace UsersPresentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("current", Name = "GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid or missing user ID claim in token." });
            }

            var currentUser = await _userService.GetUserByIdAsync(userId, cancellationToken);

            if (currentUser is null)
            {
                return NotFound(new { message = "User not found." });
            }

            currentUser.Role = User.FindAll("role").Select(c => c.Value).ToArray();

            return Ok(currentUser);
        }

        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = $"{AppRoles.Doctor},{AppRoles.Admin}")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:guid}", Name = "UserById")]
        [Authorize(Roles = $"{AppRoles.Doctor},{AppRoles.Admin}")]
        public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);

            if (user is null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("profile")]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileDto profileDto)
        {
            var result = await _userService.CreateUserProfileAsync(profileDto);

            if (!result)
            {
                return BadRequest(new { message = "User profile creation failed." });
            }

            return Created();
        }

        [HttpDelete("profile")]
        [Authorize(Roles = $"{AppRoles.Admin}")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
                return BadRequest(new { error = "Something went wrong during user deletion." });

            return NoContent();
        }
    }
}
