using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Blog_API.Modules.Users
{
    public static class ClaimPrincipalExtentionMethod
    {
        public static string? Email (this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email, StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }
}
