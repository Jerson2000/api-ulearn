
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ULearn.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Safely gets the user ID from the claims. Returns null if not found or invalid.
    /// </summary>
    public static Guid? GetUserIdSafe(this ClaimsPrincipal user)
    {
        if (user == null)
            return null;

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return null;

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            return null;

        return userId;
    }
}