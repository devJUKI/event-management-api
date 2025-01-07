using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Domain.Entities;

namespace EventManagementAPI.Infrastructure.Services;

public class EventInfrastructureService : IEventInfrastructureService
{
    private readonly IEventRepository _eventRepository;
    private readonly ICategoryRepository _categoryRepository;

    public EventInfrastructureService(
        IEventRepository eventRepository, 
        ICategoryRepository categoryRepository)
    {
        _eventRepository = eventRepository;
        _categoryRepository = categoryRepository;
    }

    public Task<PaginatedList<EventDomainModel>> GetEventsAsync(int pageSize, int page, EventFilters? filters = null, CancellationToken cancellation = default)
    {
        return _eventRepository.GetAsync(pageSize, page, filters, cancellation);
    }

    public Task<EventDomainModel?> GetEventAsync(Guid Id, CancellationToken cancellation = default)
    {
        return _eventRepository.GetAsync(Id, cancellation);
    }

    public Task<List<CategoryDomainModel>> GetCategories(List<int>? ids = null, CancellationToken cancellation = default)
    {
        return _categoryRepository.GetCategories(ids, cancellation);
    }

    public Task<Guid> CreateEventAsync(EventDomainModel userModel, CancellationToken cancellation = default)
    {
        return _eventRepository.InsertAsync(userModel, cancellation);
    }

    public Task UpdateEventAsync(EventDomainModel userModel, CancellationToken cancellation = default)
    {
        return _eventRepository.UpdateAsync(userModel, cancellation);
    }

    public Task DeleteEventAsync(Guid id, CancellationToken cancellation = default)
    {
        return _eventRepository.DeleteAsync(id, cancellation);
    }

    public Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        return _eventRepository.SaveChangesAsync(cancellation);
    }
}
