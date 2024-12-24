using EventManagementAPI.ViewModels.Common;
using EventManagementAPI.Domain.Entities;
using EventManagementAPI.Domain.Models.UserManagement;

namespace EventManagementAPI.Domain.Interfaces;

public interface IUserService
{
    Task<Result<UserResponseViewModel>> GetUser(Guid userId, CancellationToken cancellation);
    Task<Result<UserResponseViewModel>> UpdateUser(Guid userId, UpdateUserRequestDomainModel domainModel, CancellationToken cancellation);
}
