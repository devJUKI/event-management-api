using EventManagementAPI.Domain.Entities;
using EventManagementAPI.Domain.Models.Authentication;
using EventManagementAPI.ViewModels.Authentication;

namespace EventManagementAPI.Domain.Interfaces;

public interface IAuthenticationService
{
    Task<Result<AuthResponseViewModel>> Register(RegisterDomainModel domainModel, CancellationToken cancellation = default);
    Task<Result<AuthResponseViewModel>> Login(LoginDomainModel domainModel, CancellationToken cancellation = default);
}
