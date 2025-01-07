using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IUserInfrastructureService
{
    Task<UserDomainModel?> GetUserAsync(Guid Id, CancellationToken cancellation = default);
    Task<UserDomainModel?> GetUserByEmailAsync(string email, CancellationToken cancellation = default);
    Task<bool> ExistsByEmailOrUsernameAsync(string email, string username, CancellationToken cancellation = default);
    Task<Guid> CreateUserAsync(UserDomainModel userModel, CancellationToken cancellation = default);
    Task UpdateUserAsync(UserDomainModel userModel, CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellation = default);
}
