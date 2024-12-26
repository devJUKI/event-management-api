namespace EventManagementAPI.Domain.Interfaces;

public interface IAuthorizationService
{
    bool IsAuthorized(Guid ownerGuid);
}
