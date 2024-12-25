using EventManagementAPI.Domain.Models.UserManagement;
using EventManagementAPI.ViewModels.Authentication;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.ViewModels.Common;
using EventManagementAPI.Domain.Entities;
using System.Net;

namespace EventManagementAPI.Domain.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUserInfrastructureService _infrastructureService;
    private readonly IPasswordHasher _passwordHasher;

    public UserManagementService(IUserInfrastructureService userInfrastructureService, IPasswordHasher passwordHasher)
    {
        _infrastructureService = userInfrastructureService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserResponseViewModel>> GetUser(Guid userId, CancellationToken cancellation)
    {
        var user = await _infrastructureService.GetUserAsync(userId, cancellation);

        if (user == null)
        {
            return Result.Failure<UserResponseViewModel>(new Error(HttpStatusCode.NotFound, "User was not found."));
        }

        var userResponse = new UserResponseViewModel(user.Id, user.Username, user.Email, user.CreatedAt, user.Roles);

        return Result.Success(userResponse);
    }

    public async Task<Result<UserResponseViewModel>> UpdateUser(Guid userId, UpdateUserRequestDomainModel domainModel, CancellationToken cancellation)
    {
        var user = await _infrastructureService.GetUserAsync(userId, cancellation);

        if (user == null)
        {
            return Result.Failure<UserResponseViewModel>(new Error(HttpStatusCode.NotFound, "User was not found."));
        }

        var userExists = await _infrastructureService.ExistsByEmailOrUsernameAsync(domainModel.Email, domainModel.Username, cancellation);

        if (userExists)
        {
            return Result.Failure<UserResponseViewModel>(new Error(HttpStatusCode.Conflict, "User with this email/username already exists."));
        }

        var response = user.Update(domainModel.Username, domainModel.Email);

        if (response.IsFailure)
        {
            return Result.Failure<UserResponseViewModel>(response.Error);
        }

        await _infrastructureService.UpdateUserAsync(user, cancellation);
        await _infrastructureService.SaveChangesAsync(cancellation);

        var userResponse = new UserResponseViewModel(user.Id, user.Username, user.Email, user.CreatedAt, user.Roles);

        return Result.Success(userResponse);
    }
}
