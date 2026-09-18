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
        private readonly ILogger<InternalUsersController> _logger;

        public InternalUsersController(IUserService userService, ILogger<InternalUsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "UserById")]
        public async Task<IActionResult> GetUser(string id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting to fetch user {Id}...", id);

                var user = await _userService.GetUserByIdAsync(id, cancellationToken);
                return Ok(user);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Request for user {Id} was cancelled by the client!", id);

                return StatusCode(499, "Request cancelled");
            }
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetUserForAuth(string email)
        {
            var user = await _userService.GetUserForAuthByEmailAsync(email);
            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("profile")]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileDto profileDto)
        {
            if (profileDto == null || string.IsNullOrEmpty(profileDto.Id))
            {
                _logger.LogWarning("Profile creation failed: Payload is null or ID is missing.");
                return BadRequest("Invalid profile payload.");
            }

            var result = await _userService.CreateUserProfileAsync(profileDto);

            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToList();

                _logger.LogWarning("Failed profile creation for ID {Id}. Reasons: {Errors}",
                    profileDto.Id,
                    string.Join(", ", errorMessages));

                return BadRequest(new
                {
                    message = "User profile creation failed.",
                    errors = errorMessages
                });
            }

            _logger.LogInformation("User profile successfully created for ID: {Id}", profileDto.Id);
            return StatusCode(201, new { message = "User profile created successfully." });
        }

        [HttpDelete("profile")]
        public async Task<IActionResult> DeleteUserAsync([FromBody] string Id)
        {
            var result = await _userService.DeleteUserAsync(Id);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
