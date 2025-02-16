﻿using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.ViewModels.EventManagement;
using EventManagementAPI.Domain.Entities;
using EventManagementAPI.ViewModels.Common;

namespace EventManagementAPI.Domain.Interfaces;

public interface IEventManagementService
{
    Task<Result<PaginatedListResponseViewModel<EventResponseViewModel>>> GetEvents(int page, int pageSize, EventFilters? filters = null, CancellationToken cancellation = default);
    Task<Result<EventResponseViewModel>> GetEvent(Guid eventId, CancellationToken cancellation = default);
    Task<Result<List<CategoryResponseViewModel>>> GetCategories(List<int>? ids = null, CancellationToken cancellation = default);
    Task<Result<EventResponseViewModel>> CreateEvent(CreateEventDomainModel domainModel, CancellationToken cancellation = default);
    Task<Result<EventResponseViewModel>> UpdateEvent(UpdateEventDomainModel domainModel, CancellationToken cancellation = default);
    Task<Result> DeleteEvent(Guid eventId, CancellationToken cancellation = default);
}
