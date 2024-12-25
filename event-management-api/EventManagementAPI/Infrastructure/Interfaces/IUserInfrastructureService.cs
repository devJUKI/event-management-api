using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IUserInfrastructureService
{
    Task<UserDomainModel?> GetUserAsync(Guid Id, CancellationToken cancellation);
    Task<UserDomainModel?> GetUserByEmailAsync(string email, CancellationToken cancellation);
    Task<bool> ExistsByEmailOrUsernameAsync(string email, string username, CancellationToken cancellation);
    Task CreateUserAsync(UserDomainModel userModel, CancellationToken cancellation);
    Task UpdateUserAsync(UserDomainModel userModel, CancellationToken cancellation);
    Task SaveChangesAsync(CancellationToken cancellation);
}
