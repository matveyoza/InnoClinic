using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared.DataTransferObjects;

namespace UsersPresentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class InternalUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public InternalUsersController(IUserService userService, ILogger<InternalUsersController> logger) =>
            _userService = userService;

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:guid}", Name = "UserById")]
        public async Task<IActionResult> GetUser(Guid id, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserByIdAsync(id, cancellationToken);

            if (!result.IsSuccess)
            {
                return NotFound(new { error = result.Error.Code, message = result.Error.Message });
            }

            return Ok(result.Value);
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
        public async Task<IActionResult> DeleteUserAsync([FromBody] Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
                return BadRequest(new { error = "Something went wrong during user deletion." });

            return NoContent();
        }
    }
}
