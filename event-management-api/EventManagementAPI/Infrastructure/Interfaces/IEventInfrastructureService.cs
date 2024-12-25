using EventManagementAPI.Domain.Models.EventManagement;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IEventInfrastructureService
{
    Task<EventDomainModel?> GetEventAsync(Guid Id, CancellationToken cancellation);
    Task<List<string>> GetCategories(List<int> ids, CancellationToken cancellation);
    Task CreateEventAsync(EventDomainModel eventModel, CancellationToken cancellation);
    Task UpdateEventAsync(EventDomainModel eventModel, CancellationToken cancellation);
    Task SaveChangesAsync(CancellationToken cancellation);
}
