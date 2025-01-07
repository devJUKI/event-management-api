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
        bool isAdmin = _httpContextAccessor.IsAdmin();

        if (isAdmin)
        {
            return true;
        }

        var userId = _httpContextAccessor.GetUserId();

        return ownerGuid == userId;
    }
}
