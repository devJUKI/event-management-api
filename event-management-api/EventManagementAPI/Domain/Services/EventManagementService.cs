using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.ViewModels.EventManagement;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Constants;
using EventManagementAPI.Domain.Entities;
using System.Net;

namespace EventManagementAPI.Domain.Services;

public class EventManagementService : IEventManagementService
{
    private readonly IEventInfrastructureService _eventInfrastructureService;
    private readonly IUserInfrastructureService _userInfrastructureService;

    public EventManagementService(
        IEventInfrastructureService eventInfrastructureService, 
        IUserInfrastructureService userInfrastructureService)
    {
        _eventInfrastructureService = eventInfrastructureService;
        _userInfrastructureService = userInfrastructureService;
    }

    public async Task<Result<EventResponseViewModel>> CreateEvent(Guid userId, CreateEventDomainModel domainModel, CancellationToken cancellation)
    {
        if (userId != domainModel.CreatedById)
        {
            return Result.Failure<EventResponseViewModel>(new Error(HttpStatusCode.Unauthorized, "Unauthorized."));
        }

        var user = await _userInfrastructureService.GetUserAsync(domainModel.CreatedById, cancellation);

        if (user == null)
        {
            return Result.Failure<EventResponseViewModel>(new Error(HttpStatusCode.NotFound, "Specified user was not found."));
        }

        var distinctCategoryIds = domainModel.CategoryIds
            .Distinct()
            .ToList();
        var categories = await _eventInfrastructureService.GetCategories(distinctCategoryIds, cancellation);

        if (distinctCategoryIds.Count > categories.Count)
        {
            return Result.Failure<EventResponseViewModel>(new Error(HttpStatusCode.NotFound, "Specified category/categories doesn't exist"));
        }

        var eventResult = EventDomainModel.Create(
            domainModel.Title,
            domainModel.Description,
            domainModel.Location,
            domainModel.Date,
            user,
            domainModel.IsPublic,
            categories);

        if (eventResult.IsFailure)
        {
            return Result.Failure<EventResponseViewModel>(eventResult.Error);
        }

        var @event = eventResult.Payload;

        await _eventInfrastructureService.CreateEventAsync(@event, cancellation);
        await _eventInfrastructureService.SaveChangesAsync(cancellation);

        var response = new EventResponseViewModel(
            @event.Id,
            @event.Title,
            @event.Description,
            @event.Location,
            @event.Date,
            @event.CreatedBy.Username,
            @event.IsPublic,
            @event.Categories);

        return Result.Success(response);
    }
}