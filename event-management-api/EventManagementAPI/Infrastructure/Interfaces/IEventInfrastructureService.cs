using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Domain.Entities;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IEventInfrastructureService
{
    Task<PaginatedList<EventDomainModel>> GetEventsAsync(int pageSize, int page, EventFilters? filters = null, CancellationToken cancellation = default);
    Task<EventDomainModel?> GetEventAsync(Guid Id, CancellationToken cancellation = default);
    Task<List<CategoryDomainModel>> GetCategories(List<int>? ids = null, CancellationToken cancellation = default);
    Task CreateEventAsync(EventDomainModel eventModel, CancellationToken cancellation = default);
    Task UpdateEventAsync(EventDomainModel eventModel, CancellationToken cancellation = default);
    Task DeleteEventAsync(Guid id, CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellation = default);
}
