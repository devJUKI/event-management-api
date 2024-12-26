using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Extensions;

namespace EventManagementAPI.Domain.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthorized(Guid ownerGuid)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null");
        }

        bool isAdmin = httpContext.IsAdmin();

        if (isAdmin)
        {
            return true;
        }

        var userId = httpContext.GetUserId();

        return ownerGuid == userId;
    }
}
