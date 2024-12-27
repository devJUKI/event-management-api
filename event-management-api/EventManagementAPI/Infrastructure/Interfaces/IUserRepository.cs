using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<UserDomainModel?> GetAsync(Guid Id, CancellationToken cancellation = default);
    Task<UserDomainModel?> GetByEmailAsync(string email, CancellationToken cancellation = default);
    Task<bool> ExistsByEmailOrUsernameAsync(string email, string username, CancellationToken cancellation = default);
    Task InsertAsync(UserDomainModel userModel, CancellationToken cancellation = default);
    Task UpdateAsync(UserDomainModel userModel, CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellation = default);
}