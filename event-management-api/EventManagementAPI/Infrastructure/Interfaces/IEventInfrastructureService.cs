using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IEventInfrastructureService
{
    Task<PaginatedList<EventDomainModel>> GetEventsAsync(int pageSize, int page, CancellationToken cancellation);
    Task<EventDomainModel?> GetEventAsync(Guid Id, CancellationToken cancellation);
    Task<List<string>> GetCategories(List<int> ids, CancellationToken cancellation);
    Task CreateEventAsync(EventDomainModel eventModel, CancellationToken cancellation);
    Task UpdateEventAsync(EventDomainModel eventModel, CancellationToken cancellation);
    Task DeleteEventAsync(Guid id, CancellationToken cancellation);
    Task SaveChangesAsync(CancellationToken cancellation);
}
