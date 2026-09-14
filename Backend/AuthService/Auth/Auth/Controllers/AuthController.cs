using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

            return Ok(new { message = "Login successful" });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            return Ok(new { id = userId, email, userName });
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
