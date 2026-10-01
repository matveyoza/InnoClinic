using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.Shared;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto registerDto)
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
            var user = await _authService.ValidateUserAsync(userDto);
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

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
            });
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
