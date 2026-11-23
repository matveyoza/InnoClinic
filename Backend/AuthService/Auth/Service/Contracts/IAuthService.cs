using Microsoft.AspNetCore.Identity;
using Entities.Models;
using Service.Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(UserForRegistrationDto registerDto);
        Task<User?> AuthenticateUserAsync(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateTokenAsync(User user);
    }
}
