using EventManagementAPI.Domain.Models.Authentication;
using EventManagementAPI.Infrastructure.Interfaces;

namespace EventManagementAPI.Infrastructure.Services;

public class AuthenticationInfrastructureService : IAuthenticationInfrastructureService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationInfrastructureService(IUserRepository userRepository)
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

    public Task<UserDomainModel?> GetUserByEmailAndUsernameAsync(string email, string username, CancellationToken cancellation)
    {
        return _userRepository.GetByEmailAndUsernameAsync(email, username, cancellation);
    }

    public Task CreateUserAsync(UserDomainModel userModel, CancellationToken cancellation)
    {
        return _userRepository.InsertAsync(userModel, cancellation);
    }

    public Task SaveChangesAsync(CancellationToken cancellation)
    {
        return _userRepository.SaveChangesAsync(cancellation);
    }
}
