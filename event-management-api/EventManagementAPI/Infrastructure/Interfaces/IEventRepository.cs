using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Domain.Models.EventManagement;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IEventRepository
{
    Task<PaginatedList<EventDomainModel>> GetAsync(int pageSize, int page, CancellationToken cancellation);
    Task<EventDomainModel?> GetAsync(Guid Id, CancellationToken cancellation);
    Task InsertAsync(EventDomainModel eventModel, CancellationToken cancellation);
    Task UpdateAsync(EventDomainModel eventModel, CancellationToken cancellation);
    Task DeleteAsync(Guid id, CancellationToken cancellation);
    Task SaveChangesAsync(CancellationToken cancellation);
}
