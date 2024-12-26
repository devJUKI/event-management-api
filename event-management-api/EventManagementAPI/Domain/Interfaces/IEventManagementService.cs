using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.ViewModels.EventManagement;
using EventManagementAPI.Domain.Entities;
using EventManagementAPI.ViewModels.Common;

namespace EventManagementAPI.Domain.Interfaces;

public interface IEventManagementService
{
    Task<Result<PaginatedListResponseViewModel<EventResponseViewModel>>> GetEvents(int page, int pageSize, CancellationToken cancellation);
    Task<Result<EventResponseViewModel>> GetEvent(Guid eventId, CancellationToken cancellation);
    Task<Result<EventResponseViewModel>> CreateEvent(CreateEventDomainModel domainModel, CancellationToken cancellation);
    Task<Result<EventResponseViewModel>> UpdateEvent(UpdateEventDomainModel domainModel, CancellationToken cancellation);
    Task<Result> DeleteEvent(Guid eventId, CancellationToken cancellation);
}
