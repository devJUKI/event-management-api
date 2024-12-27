using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Infrastructure.Interfaces;

namespace EventManagementAPI.Infrastructure.Services;

public class UserInfrastructureService : IUserInfrastructureService
{
    private readonly IUserRepository _userRepository;

    public UserInfrastructureService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserDomainModel?> GetUserAsync(Guid Id, CancellationToken cancellation = default)
    {
        return _userRepository.GetAsync(Id, cancellation);
    }

    public Task<UserDomainModel?> GetUserByEmailAsync(string email, CancellationToken cancellation = default)
    {
        return _userRepository.GetByEmailAsync(email, cancellation);
    }

    public Task<bool> ExistsByEmailOrUsernameAsync(string email, string username, CancellationToken cancellation = default)
    {
        return _userRepository.ExistsByEmailOrUsernameAsync(email, username, cancellation);
    }

    public Task CreateUserAsync(UserDomainModel userModel, CancellationToken cancellation = default)
    {
        return _userRepository.InsertAsync(userModel, cancellation);
    }

    public Task UpdateUserAsync(UserDomainModel userModel, CancellationToken cancellation = default)
    {
        return _userRepository.UpdateAsync(userModel, cancellation);
    }

    public Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        return _userRepository.SaveChangesAsync(cancellation);
    }
}
