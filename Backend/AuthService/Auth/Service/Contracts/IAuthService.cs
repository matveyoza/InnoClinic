using Microsoft.AspNetCore.Identity;
using Service.Shared;
using Entities.Models;

namespace Service.Contracts
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(UserForRegistrationDto registerDto);
        Task<User?> AuthenticateUserAsync(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateTokenAsync(User user);
    }
}
