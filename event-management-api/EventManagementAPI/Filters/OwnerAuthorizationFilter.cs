using EventManagementAPI.Domain.Constants;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagementAPI.Filters;

public class OwnerAuthorizationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!context.HttpContext.Request.RouteValues.TryGetValue("userId", out var userIdObj))
        {
            return Results.BadRequest();
        }

        var jwtUserRoles = context.HttpContext.User.FindAll(ClaimTypes.Role);

        if (jwtUserRoles.Any(c => c.Value == Roles.Admin))
        {
            return await next(context);
        }

        var userId = userIdObj!.ToString();

        var jwtUserId = context.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (jwtUserId != userId)
        {
            var problemDetails = new ProblemDetails
            {
                Status = 401,
                Extensions = { ["errors"] = new List<string>() { "Authorization failed." } }
            };

            return Results.Problem(problemDetails);
        }

        return await next(context);
    }
}
