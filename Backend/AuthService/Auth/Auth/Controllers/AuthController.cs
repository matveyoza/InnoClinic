using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        public AuthController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserForRegistrationDto registerDto)
        {
            var result = await _authService.RegisterUserAsync(registerDto);

            if (!result)
            {
                return BadRequest(new { error = "User registation failed." });
            }

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userDto)
        {
            var user = await _authService.AuthenticateUserAsync(userDto);
            if (user is null)
                return Unauthorized(new { message = "Invalid credentials" });

            var jwtTokenDto = await _authService.CreateTokenAsync(user);
            if (string.IsNullOrEmpty(jwtTokenDto?.AccessToken))
            {
                return BadRequest(new { message = "Token generation failed" });
            }

            Response.Cookies.Append("AuthToken", jwtTokenDto.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddHours(24)
            });

            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault() ?? "Patient";

            return Ok(new
            {
                id = user.Id,
                userName = user.UserName ?? user.Email,
                email = user.Email,
                role = primaryRole
            });
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