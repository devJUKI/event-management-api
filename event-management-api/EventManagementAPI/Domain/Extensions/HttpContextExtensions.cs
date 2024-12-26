using EventManagementAPI.Domain.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EventManagementAPI.Domain.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (claim == null || claim.Value == null)
        {
            throw new UnauthorizedAccessException("Unauthorized.");
        }
        
        return new Guid(claim.Value);
    }

    public static bool IsAdmin(this HttpContext context)
    {
        var claims = context.GetRoles();

        return claims.Contains(Roles.Admin);
    }

    public static List<string> GetRoles(this HttpContext context)
    {
        var claims = context.User.FindAll(ClaimTypes.Role);

        if (claims == null)
        {
            throw new UnauthorizedAccessException("Roles are not specified.");
        }

        return claims.Select(c => c.Value).ToList();
    }
}
