using Microsoft.AspNetCore.Identity;
using Auth.Service.Shared;
using Auth.Entities.Models;

namespace Auth.Service.Contracts
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(UserForRegistrationDto registerDto);
        Task<User?> AuthenticateUserAsync(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateTokenAsync(User user);
    }
}
