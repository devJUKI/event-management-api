using EventManagementAPI.Domain.Models.Attendance;
using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IAttendanceInfrastructureService
{
    Task<PaginatedList<AttendanceDomainModel>> GetAttendancesAsync(Guid eventId, int pageSize, int page, CancellationToken cancellation = default);
    Task<AttendanceDomainModel?> GetAttendanceAsync(Guid eventId, Guid userId, CancellationToken cancellation = default);
    Task CreateAttendanceAsync(AttendanceDomainModel attendanceModel, CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellation = default);
}
