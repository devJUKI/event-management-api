using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Domain.Entities;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IEventRepository
{
    Task<PaginatedList<EventDomainModel>> GetAsync(int pageSize, int page, EventFilters? filters = null, CancellationToken cancellation = default);
    Task<EventDomainModel?> GetAsync(Guid Id, CancellationToken cancellation = default);
    Task<Guid> InsertAsync(EventDomainModel eventModel, CancellationToken cancellation = default);
    Task UpdateAsync(EventDomainModel eventModel, CancellationToken cancellation = default);
    Task DeleteAsync(Guid id, CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellation = default);
}
