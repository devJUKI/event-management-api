using EventManagementAPI.Domain.Models.Authentication;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IAuthenticationInfrastructureService
{
    Task<UserDomainModel?> GetUserAsync(Guid Id, CancellationToken cancellation);
    Task<UserDomainModel?> GetUserByEmailAsync(string email, CancellationToken cancellation);
    Task<UserDomainModel?> GetUserByEmailAndUsernameAsync(string email, string username, CancellationToken cancellation);
    Task CreateUserAsync(UserDomainModel userModel, CancellationToken cancellation);
    Task SaveChangesAsync(CancellationToken cancellation);
}
