using System.IdentityModel.Tokens.Jwt;

namespace EventManagementAPI.Domain.Extensions;

public static class HttpContextExtensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirst(JwtRegisteredClaimNames.Sub);

        if (claim == null || claim.Value == null)
        {
            return null;
        }
        
        return new Guid(claim.Value);
    }
}
