using Microsoft.AspNetCore.Identity;
using Service.Shared;

namespace Service.Contracts
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto);
        Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuth);
        Task<TokenDto> CreateTokenAsync();
    }
}
