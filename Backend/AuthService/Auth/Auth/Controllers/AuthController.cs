using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared;
using System.Net;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            if (registerDto is null)
                return BadRequest("User registration payload is null.");

            var result = await _authService.RegisterUserAsync(registerDto);

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

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authService.ValidateUserAsync(user))
                return Unauthorized("Invalid email or password.");

            var tokenDto = await _authService.CreateTokenAsync();

            return Ok(tokenDto);
        }
    }
}
