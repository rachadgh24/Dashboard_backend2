using System.Security.Claims;

namespace Test2.Helpers
{
    public static class UserClaimsHelper
    {
        /// <summary>
        /// Resolves the display name from the authenticated user's claims.
        /// Tries, in order: Name, name, unique_name, GivenName, then email fallbacks, then "Someone".
        /// </summary>
        public static string GetDisplayName(ClaimsPrincipal user)
        {
            if (user == null) return "Someone";

            var value = user.FindFirst(ClaimTypes.Name)?.Value
                ?? user.FindFirst("name")?.Value
                ?? user.FindFirst("unique_name")?.Value
                ?? user.FindFirst(ClaimTypes.GivenName)?.Value;

            if (!string.IsNullOrWhiteSpace(value)) return value.Trim();

            value = user.FindFirst(ClaimTypes.Email)?.Value
                ?? user.FindFirst("email")?.Value;

            return string.IsNullOrWhiteSpace(value) ? "Someone" : value.Trim();
        }

        /// <summary>
        /// Resolves the role from the authenticated user's claims.
        /// </summary>
        public static string GetRole(ClaimsPrincipal user)
        {
            if (user == null) return "User";
            var role = user.FindFirst(ClaimTypes.Role)?.Value
                ?? user.FindFirst("role")?.Value;
            return string.IsNullOrWhiteSpace(role) ? "User" : role.Trim();
        }
    }
}
