using EventManagementAPI.Domain.Extensions;
using EventManagementAPI.Domain.Helpers;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Models.Authentication;
using EventManagementAPI.ViewModels.Authentication;

namespace EventManagementAPI.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        var mapGroup = app
            .MapGroup("/auth")
            .WithTags("Authentication");

        mapGroup.MapPost("/register", async (
            RegisterRequestViewModel viewModel,
            IAuthenticationService service,
            CancellationToken cancellationToken) =>
        {
            var domainModel = new RegisterDomainModel
            {
                Username = viewModel.Username,
                Email = viewModel.Email,
                Password = viewModel.Password
            };

            var result = await service.Register(domainModel, cancellationToken);

            return result.Match(CustomResults.Ok, CustomResults.Problem);
        });

        mapGroup.MapPost("/login", async (
            LoginRequestViewModel viewModel,
            IAuthenticationService service,
            CancellationToken cancellationToken) =>
        {
            var domainModel = new LoginDomainModel
            {
                Email = viewModel.Email,
                Password = viewModel.Password
            };

            var result = await service.Login(domainModel, cancellationToken);

            return result.Match(CustomResults.Ok, CustomResults.Problem);
        });
    }
}
