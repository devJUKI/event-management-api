using EventManagementAPI.Domain.Models.UserManagement;
using EventManagementAPI.ViewModels.UserManagement;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Extensions;
using EventManagementAPI.Domain.Helpers;
using EventManagementAPI.Filters;

namespace EventManagementAPI.Endpoints;

public static class UserManagementEndpoints
{
    public static void MapUserManagementEndpoints(this WebApplication app)
    {
        var mapGroup = app
            .MapGroup("/users")
            .WithTags("User Management");

        mapGroup.MapGet("/{userId}", async (
            Guid userId,
            IUserManagementService userService,
            CancellationToken cancellation) =>
        {
            var result = await userService.GetUser(userId, cancellation);

            return result.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .AddEndpointFilter<OwnerAuthorizationFilter>();

        mapGroup.MapPut("/{userId}", async (
            Guid userId,
            UpdateUserRequestViewModel viewModel,
            IUserManagementService userService,
            CancellationToken cancellation) =>
        {
            var domainModel = new UpdateUserRequestDomainModel
            {
                Email = viewModel.Email!,
                Username = viewModel.Username!
            };

            var result = await userService.UpdateUser(userId, domainModel, cancellation);

            return result.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .AddEndpointFilter<ValidationFilter<UpdateUserRequestViewModel>>()
        .AddEndpointFilter<OwnerAuthorizationFilter>();
    }
}
