using EventManagementAPI.ViewModels.Common;
using EventManagementAPI.Domain.Entities;
using EventManagementAPI.Domain.Models.UserManagement;

namespace EventManagementAPI.Domain.Interfaces;

public interface IUserManagementService
{
    Task<Result<UserResponseViewModel>> GetUser(Guid userId, CancellationToken cancellation = default);
    Task<Result<UserResponseViewModel>> UpdateUser(Guid userId, UpdateUserRequestDomainModel domainModel, CancellationToken cancellation = default);
}
