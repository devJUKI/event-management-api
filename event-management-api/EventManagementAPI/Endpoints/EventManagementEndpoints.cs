using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.ViewModels.EventManagement;
using EventManagementAPI.Domain.Extensions;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Helpers;
using System.IdentityModel.Tokens.Jwt;
using EventManagementAPI.Filters;

namespace EventManagementAPI.Endpoints;

public static class EventManagementEndpoints
{
    public static void MapEventManagementEndpoints(this WebApplication app)
    {
        var mapGroup = app
            .MapGroup("/events")
            .WithTags("Event Management");

        mapGroup.MapPost("/", async (
            CreateEventRequestViewModel viewModel, 
            HttpContext context, 
            IEventManagementService eventService,
            CancellationToken cancellation) =>
        {
            var userId = context.GetUserId();

            var domainModel = new CreateEventDomainModel
            {
                Title = viewModel.Title!,
                Description = viewModel.Description,
                Location = viewModel.Location!,
                Date = viewModel.Date!.Value,
                CreatedById = viewModel.CreatedById!.Value,
                IsPublic = viewModel.IsPublic!.Value,
                CategoryIds = viewModel.CategoryIds!
            };

            var response = await eventService.CreateEvent(userId!.Value, domainModel, cancellation);

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .AddEndpointFilter<ValidationFilter<CreateEventRequestViewModel>>()
        .RequireAuthorization();
    }
}
