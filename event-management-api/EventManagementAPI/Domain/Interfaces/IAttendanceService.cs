using EventManagementAPI.Domain.Entities;
using EventManagementAPI.Domain.Models.Attendance;
using EventManagementAPI.ViewModels.Attendance;
using EventManagementAPI.ViewModels.Common;

namespace EventManagementAPI.Domain.Interfaces;

public interface IAttendanceService
{
    Task<Result<AttendanceResponseViewModel>> CreateAttendance(CreateAttendanceDomainModel domainModel, CancellationToken cancellation = default);
    Task<Result<PaginatedListResponseViewModel<UserAttendanceResponseViewModel>>> GetAttendees(Guid eventId, int page, int pageSize, CancellationToken cancellation = default);
}
