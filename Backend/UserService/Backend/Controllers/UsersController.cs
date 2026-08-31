using Entities.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared.DataTransferObjects;

namespace UsersPresentation.Controllers
{
    [Route("api/users/internal")]
    [ApiController]
    public class InternalUsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public InternalUsersController(IUserService userService) =>
            _userService = userService;

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "UserById")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("internal/by-email/{email}")]
        public async Task<IActionResult> GetUserForAuth(string email)
        {
            var user = await _userService.GetUserForAuthByEmailAsync(email);
            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("profile")]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileDto profileDto)
        {
            if (profileDto == null || string.IsNullOrEmpty(profileDto.Id))
                return BadRequest("Invalid profile payload.");

            var result = await _userService.CreateUserProfileAsync(profileDto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return StatusCode(201);
        }
    }
}
