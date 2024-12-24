using EventManagementAPI.Domain.Models;
using EventManagementAPI.Infrastructure.Interfaces;

namespace EventManagementAPI.Infrastructure.Services;

public class UserInfrastructureService : IUserInfrastructureService
{
    private readonly IUserRepository _userRepository;

    public UserInfrastructureService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<UserDomainModel?> GetUserAsync(Guid Id, CancellationToken cancellation)
    {
        return _userRepository.GetAsync(Id, cancellation);
    }

    public Task<UserDomainModel?> GetUserByEmailAsync(string email, CancellationToken cancellation)
    {
        return _userRepository.GetByEmailAsync(email, cancellation);
    }

    public Task<bool> ExistsByEmailOrUsernameAsync(string email, string username, CancellationToken cancellation)
    {
        return _userRepository.ExistsByEmailOrUsernameAsync(email, username, cancellation);
    }

    public Task CreateUserAsync(UserDomainModel userModel, CancellationToken cancellation)
    {
        return _userRepository.InsertAsync(userModel, cancellation);
    }

    public Task UpdateUserAsync(UserDomainModel userModel, CancellationToken cancellation)
    {
        return _userRepository.UpdateAsync(userModel, cancellation);
    }

    public Task SaveChangesAsync(CancellationToken cancellation)
    {
        return _userRepository.SaveChangesAsync(cancellation);
    }
}
