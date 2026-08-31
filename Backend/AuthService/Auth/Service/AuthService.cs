using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Service.Shared;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User? _user;
        public AuthService(
            UserManager<User> userManager,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName ?? registerDto.Email,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return result;

            var profileDto = new UserProfileCreationDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = user.UserName
            };

            var profileCreated = await CreateUserProfileAsync(profileDto);

            if (!profileCreated)
            {
                await _userManager.DeleteAsync(user);
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Failed to create user profile in User Service."
                });
            }

            return IdentityResult.Success;

        }

        public async Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuth)
        {
            _user = await _userManager.FindByEmailAsync(userForAuth.Email);

            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));

            return result;
        }

        public async Task<TokenDto> CreateTokenAsync()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"]!;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _user!.Id),
                new Claim(ClaimTypes.Name, _user.UserName ?? _user.Email!),
                new Claim(ClaimTypes.Email, _user.Email!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["expiresInMinutes"])),
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto { AccessToken = accessToken };
        }

        private async Task<bool> CreateUserProfileAsync(UserProfileCreationDto profileDto)
        {
            var client = _httpClientFactory.CreateClient("UserService");

            var response = await client.PostAsJsonAsync("api/users/internal/profile", profileDto);

            return response.IsSuccessStatusCode;
        }
    }
}
