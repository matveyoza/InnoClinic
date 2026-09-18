using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Entities.Models;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            if (registerDto is null)
            {
                return BadRequest(ApiResponse<object>.Fail(
                    new List<string> { "Payload is null." },
                    "Invalid request."
                ));
            }

            var result = await _authService.RegisterUserAsync(registerDto);

            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToList();

                _logger.LogWarning("Failed registration attempt for email {Email}. Reasons: {Errors}",
                    registerDto.Email,
                    string.Join(", ", errorMessages));

                return BadRequest(ApiResponse<object>.Fail(
                    errorMessages,
                    "User registation failed."));
            }

            _logger.LogInformation("User successfully registered: {Email}", registerDto.Email);
            return StatusCode(201, ApiResponse<object?>.Ok(null, "User registered successfully."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userDto = new UserForAuthenticationDto
            {
                Email = request.Email,
                Password = request.Password
            };
            var (isValid, user) = await _authService.ValidateUserAsync(userDto);
            if (!isValid || user is null)
                return Unauthorized(new { message = "Invalid credentials" });

            var jwtTokenDto = await _authService.CreateTokenAsync(user);

            Response.Cookies.Append("AuthToken", jwtTokenDto.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddHours(24)
            });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            return Ok(new { id = userId, email, userName });
        }

        [HttpGet("check")]
        [Authorize]
        public IActionResult AuthCheck()
        {
            return Ok();
        }

        [HttpGet("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
