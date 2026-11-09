using System.Security.Claims;

namespace AppointmentPresentation.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst("sub")?.Value ?? string.Empty;

            return Guid.TryParse(idClaim, out var id) ? id : Guid.Empty;
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.FindFirst("role")?.Value ?? string.Empty;
        }
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Name)
                   ?? user.FindFirst("name")?.Value
                   ?? user.FindFirst("preferred_username")?.Value
                   ?? string.Empty;
        }
    }
}
