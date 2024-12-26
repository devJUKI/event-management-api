using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.ViewModels.EventManagement;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Interfaces;
using EventManagementAPI.Domain.Entities;
using System.Net;
using EventManagementAPI.ViewModels.Common;
using EventManagementAPI.Domain.Constants;

namespace EventManagementAPI.Domain.Services;

public class EventManagementService : IEventManagementService
{
    private readonly IEventInfrastructureService _eventInfrastructureService;
    private readonly IUserInfrastructureService _userInfrastructureService;
    private readonly IAuthorizationService _authorizationService;

    public EventManagementService(
        IEventInfrastructureService eventInfrastructureService,
        IUserInfrastructureService userInfrastructureService,
        IAuthorizationService authorizationService)
    {
        _eventInfrastructureService = eventInfrastructureService;
        _userInfrastructureService = userInfrastructureService;
        _authorizationService = authorizationService;
    }

    public async Task<Result<PaginatedListResponseViewModel<EventResponseViewModel>>> GetEvents(int page, int pageSize, CancellationToken cancellation)
    {
        if (pageSize > EventData.MaxPageSize)
        {
            return Result.Failure<PaginatedListResponseViewModel<EventResponseViewModel>>(
                new Error(HttpStatusCode.BadRequest,
                $"Page size is too big. Max size is {EventData.MaxPageSize}"));
        }

        var paginatedEvents = await _eventInfrastructureService.GetEventsAsync(pageSize, page, cancellation);

        var eventDomainList = paginatedEvents.Items.Select(e => new EventResponseViewModel(
            e.Id,
            e.Title,
            e.Description,
            e.Location,
            e.Date,
            e.CreatedBy.Username,
            e.IsPublic,
            e.Categories))
            .ToList();

        var response = new PaginatedListResponseViewModel<EventResponseViewModel>(
            paginatedEvents.Page,
            paginatedEvents.PageSize,
            paginatedEvents.TotalPages,
            paginatedEvents.TotalCount,
            paginatedEvents.HasPreviousPage,
            paginatedEvents.HasNextPage,
            eventDomainList);

        return Result.Success(response);
    }

    public async Task<Result<EventResponseViewModel>> GetEvent(Guid eventId, CancellationToken cancellation)
    {
        var @event = await _eventInfrastructureService.GetEventAsync(eventId, cancellation);

        if (@event == null)
        {
            return Result.Failure<EventResponseViewModel>(new Error(HttpStatusCode.NotFound, "Event with specified id was not found."));
        }

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

    public async Task<Result<EventResponseViewModel>> CreateEvent(CreateEventDomainModel domainModel, CancellationToken cancellation)
    {
        if (!_authorizationService.IsAuthorized(domainModel.CreatedById))
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

    public async Task<Result<EventResponseViewModel>> UpdateEvent(UpdateEventDomainModel domainModel, CancellationToken cancellation)
    {
        var @event = await _eventInfrastructureService.GetEventAsync(domainModel.EventId, cancellation);

        if (@event == null)
        {
            return Result.Failure<EventResponseViewModel>(new Error(HttpStatusCode.NotFound, "Event was not found."));
        }

        if (!_authorizationService.IsAuthorized(@event.CreatedBy.Id))
        {
            return Result.Failure<EventResponseViewModel>(new Error(HttpStatusCode.Unauthorized, "Unauthorized."));
        }

        var distinctCategoryIds = domainModel.CategoryIds
            .Distinct()
            .ToList();
        var categories = await _eventInfrastructureService.GetCategories(distinctCategoryIds, cancellation);

        if (distinctCategoryIds.Count > categories.Count)
        {
            return Result.Failure<EventResponseViewModel>(new Error(HttpStatusCode.NotFound, "Specified category/categories doesn't exist"));
        }

        @event.Update(
            domainModel.Title,
            domainModel.Description,
            domainModel.Location,
            domainModel.Date,
            domainModel.IsPublic,
            categories);

        await _eventInfrastructureService.UpdateEventAsync(@event, cancellation);
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

    public async Task<Result> DeleteEvent(Guid eventId, CancellationToken cancellation)
    {
        await _eventInfrastructureService.DeleteEventAsync(eventId, cancellation);
        await _eventInfrastructureService.SaveChangesAsync(cancellation);

        return Result.Success();
    }
}