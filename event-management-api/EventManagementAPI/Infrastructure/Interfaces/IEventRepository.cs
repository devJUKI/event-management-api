using EventManagementAPI.Domain.Models.EventManagement;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IEventRepository
{
    Task<EventDomainModel?> GetAsync(Guid Id, CancellationToken cancellation);
    Task InsertAsync(EventDomainModel eventModel, CancellationToken cancellation);
    Task UpdateAsync(EventDomainModel eventModel, CancellationToken cancellation);
    Task SaveChangesAsync(CancellationToken cancellation);
}
