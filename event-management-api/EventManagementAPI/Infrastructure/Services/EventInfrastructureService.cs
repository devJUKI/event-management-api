using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Infrastructure.Interfaces;

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

    public Task<EventDomainModel?> GetEventAsync(Guid Id, CancellationToken cancellation)
    {
        return _eventRepository.GetAsync(Id, cancellation);
    }

    public Task<List<string>> GetCategories(List<int> ids, CancellationToken cancellation)
    {
        return _categoryRepository.GetCategories(ids, cancellation);
    }

    public Task CreateEventAsync(EventDomainModel userModel, CancellationToken cancellation)
    {
        return _eventRepository.InsertAsync(userModel, cancellation);
    }

    public Task UpdateEventAsync(EventDomainModel userModel, CancellationToken cancellation)
    {
        return _eventRepository.UpdateAsync(userModel, cancellation);
    }

    public Task SaveChangesAsync(CancellationToken cancellation)
    {
        return _eventRepository.SaveChangesAsync(cancellation);
    }

    public override bool Equals(object? obj)
    {
        return obj is EventInfrastructureService service &&
               EqualityComparer<ICategoryRepository>.Default.Equals(_categoryRepository, service._categoryRepository);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_categoryRepository);
    }
}
