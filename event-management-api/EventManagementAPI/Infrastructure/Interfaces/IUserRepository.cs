using EventManagementAPI.Domain.Models.Authentication;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<UserDomainModel?> GetAsync(Guid Id, CancellationToken cancellation);
    Task<UserDomainModel?> GetByEmailAsync(string email, CancellationToken cancellation);
    Task<UserDomainModel?> GetByEmailAndUsernameAsync(string email, string username, CancellationToken cancellation);
    Task InsertAsync(UserDomainModel userModel, CancellationToken cancellation);
    Task SaveChangesAsync(CancellationToken cancellation);
}
