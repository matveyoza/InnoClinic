using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Service.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class AuthService : IAuthService
    {
        private const string UserServiceHttpClientName = "UserService";
        private const string UserProfileEndpoint = "api/users/profile";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
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

        public async Task<(bool IsValid, User? user)> ValidateUserAsync(UserForAuthenticationDto userForAuth)
        {
            var user = await _userManager.FindByEmailAsync(userForAuth.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuth.Password))
                return (false, null);

            return (true, user);
        }

        public async Task<TokenDto> CreateTokenAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user!.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
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
            var client = _httpClientFactory.CreateClient(UserServiceHttpClientName);

            var response = await client.PostAsJsonAsync(UserProfileEndpoint, profileDto);

            return response.IsSuccessStatusCode;
        }
    }
}
