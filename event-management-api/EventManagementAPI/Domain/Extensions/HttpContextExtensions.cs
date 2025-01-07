using EventManagementAPI.Domain.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EventManagementAPI.Domain.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this IHttpContextAccessor contextAccessor)
    {
        var context = GetHttpContext(contextAccessor);

        var claim = context.User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (claim == null || claim.Value == null)
        {
            throw new UnauthorizedAccessException("Unauthorized.");
        }
        
        return new Guid(claim.Value);
    }

    public static bool IsAdmin(this IHttpContextAccessor contextAccessor)
    {
        var claims = contextAccessor.GetRoles();

        return claims.Contains(Roles.Admin);
    }

    public static List<string> GetRoles(this IHttpContextAccessor contextAccessor)
    {
        var context = GetHttpContext(contextAccessor);

        var claims = context.User.FindAll(ClaimTypes.Role);

        if (claims == null)
        {
            throw new UnauthorizedAccessException("Roles are not specified.");
        }

        return claims.Select(c => c.Value).ToList();
    }

    private static HttpContext GetHttpContext(IHttpContextAccessor contextAccessor)
    {
        var context = contextAccessor.HttpContext;

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context;
    }
}
