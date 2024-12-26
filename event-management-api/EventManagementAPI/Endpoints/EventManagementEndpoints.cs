using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.ViewModels.EventManagement;
using EventManagementAPI.Domain.Extensions;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Helpers;
using EventManagementAPI.Filters;

namespace EventManagementAPI.Endpoints;

public static class EventManagementEndpoints
{
    public static void MapEventManagementEndpoints(this WebApplication app)
    {
        var mapGroup = app
            .MapGroup("/events")
            .WithTags("Event Management");

        mapGroup.MapGet("/", async (
            int page,
            int pageSize,
            IEventManagementService eventService,
            CancellationToken cancellation) =>
        {
            var response = await eventService.GetEvents(page, pageSize, cancellation);

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        });

        mapGroup.MapGet("/{eventId}", async (
            Guid eventId,
            IEventManagementService eventService,
            CancellationToken cancellation) =>
        {
            var response = await eventService.GetEvent(eventId, cancellation);

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        });

        mapGroup.MapPost("/", async (
            CreateEventRequestViewModel viewModel, 
            IEventManagementService eventService,
            CancellationToken cancellation) =>
        {
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

            var response = await eventService.CreateEvent(domainModel, cancellation);

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .AddEndpointFilter<ValidationFilter<CreateEventRequestViewModel>>()
        .RequireAuthorization();

        mapGroup.MapPut("/{eventId}", async (
            Guid eventId,
            UpdateEventRequestViewModel viewModel,
            IEventManagementService eventService,
            CancellationToken cancellation) =>
        {
            var domainModel = new UpdateEventDomainModel
            {
                EventId = eventId,
                Title = viewModel.Title!,
                Description = viewModel.Description,
                Location = viewModel.Location!,
                Date = viewModel.Date!.Value,
                IsPublic = viewModel.IsPublic!.Value,
                CategoryIds = viewModel.CategoryIds!
            };

            var response = await eventService.UpdateEvent(domainModel, cancellation);

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .AddEndpointFilter<ValidationFilter<UpdateEventRequestViewModel>>()
        .RequireAuthorization();


        mapGroup.MapDelete("/{eventId}", async (
            Guid eventId,
            IEventManagementService eventService,
            CancellationToken cancellation) =>
        {
            var response = await eventService.DeleteEvent(eventId, cancellation);

            return response.Match(CustomResults.Ok, CustomResults.Problem);
        })
        .RequireAuthorization();
    }
}
