namespace EventManagementAPI.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(Guid userId, string username, IList<string> roles);
}
