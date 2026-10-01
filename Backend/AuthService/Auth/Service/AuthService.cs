using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Service.Constants;
using Service.Contracts;
using Service.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<User> userManager,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<AuthService> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> RegisterUserAsync(UserForRegistrationDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("Failed registration attempt. Backend/AuthService/Auth/Auth/Controllers/AuthController.cs Line: 44");

                return false;
            }

            var profileDto = _mapper.Map<UserProfileCreationDto>(registerDto);
            profileDto.Id = user.Id;

            var profileCreated = await CreateUserProfileAsync(profileDto);

            if (!profileCreated)
            {
                await _userManager.DeleteAsync(user);
                _logger.LogError("User Profile Creation failed Backend/AuthService/Auth/Auth/Controllers/AuthController.cs Line: 62");
                return false;
            }

            _logger.LogInformation("User successfully registered: {Email}", registerDto.Email);
            return true;
        }

        public async Task<User?> ValidateUserAsync(UserForAuthenticationDto userForAuth)
        {
            var user = await _userManager.FindByEmailAsync(userForAuth.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuth.Password))
                return null;

            return user;
        }

        public async Task<TokenDto> CreateTokenAsync(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            var validIssuer = Environment.GetEnvironmentVariable("JWT_VALID_ISSUER");
            var validAudience = Environment.GetEnvironmentVariable("JWT_VALID_AUDIENCE");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user!.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["expiresInMinutes"])),
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto { AccessToken = accessToken };
        }

        private async Task<bool> CreateUserProfileAsync(UserProfileCreationDto profileDto)
        {
            var client = _httpClientFactory.CreateClient(AppConstants.UserServiceHttpClientName);

            var jsonString = JsonSerializer.Serialize(profileDto);

            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{AppConstants.ApiRoute}/users/profile", httpContent);
            return response.IsSuccessStatusCode;
        }
    }
}
