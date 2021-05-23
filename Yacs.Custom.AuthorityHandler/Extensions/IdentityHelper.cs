using IdentityModel;
using System.Security.Claims;

namespace Yacs.Custom.AuthorityHandler.Extensions
{
    public static class IdentityHelper
    {
        public static string GetUserId(this ClaimsPrincipal claims)
        {
            return claims.FindFirstValue(JwtClaimTypes.Subject);
        }
    }
}
