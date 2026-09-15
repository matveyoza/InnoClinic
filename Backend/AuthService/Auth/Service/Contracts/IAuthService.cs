using Microsoft.AspNetCore.Identity;
using Service.Shared;
using Entities.Models;

namespace Service.Contracts
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto);
        Task<(bool IsValid, User? user)> ValidateUserAsync(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateTokenAsync(User user);
    }
}
