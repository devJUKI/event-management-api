using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.ViewModels.EventManagement;
using EventManagementAPI.Domain.Entities;

namespace EventManagementAPI.Domain.Interfaces;

public interface IEventManagementService
{
    Task<Result<EventResponseViewModel>> CreateEvent(Guid userId, CreateEventDomainModel domainModel, CancellationToken cancellation);
}
