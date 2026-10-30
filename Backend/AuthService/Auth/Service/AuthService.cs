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
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Service;

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
            _logger.LogError("Failed registration attempt for {Email}. Errors: {Errors}",
                registerDto.Email,
                string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
        }

        var assignedRole = string.IsNullOrWhiteSpace(registerDto.Role) ? "Patient" : registerDto.Role;
        await _userManager.AddToRoleAsync(user, assignedRole);

        var profileDto = _mapper.Map<UserProfileCreationDto>(registerDto);
        profileDto.Id = user.Id;

        var profileCreated = await CreateUserProfileAsync(profileDto);

        if (!profileCreated)
        {
            await _userManager.DeleteAsync(user);
            _logger.LogError("User profile creation failed for {Email}. Rolled back user {Id}.", registerDto.Email, user.Id);
            return false;
        }

        _logger.LogInformation("User successfully registered: {Email} with role {Role}", registerDto.Email, assignedRole);
        return true;
    }

    public async Task<User?> AuthenticateUserAsync(UserForAuthenticationDto userForAuth)
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

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Email ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new("sub", user.Id)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("role", role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: validIssuer,
            audience: validAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["expiresInMinutes"] ?? "1440")),
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto { AccessToken = accessToken };
    }

    private async Task<bool> CreateUserProfileAsync(UserProfileCreationDto profileDto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("UserService");
            var jsonString = JsonSerializer.Serialize(profileDto);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{AppConstants.ApiRoute}/users/profile", httpContent);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to UserService when creating profile for {UserId}", profileDto.Id);
            return false;
        }
    }
}