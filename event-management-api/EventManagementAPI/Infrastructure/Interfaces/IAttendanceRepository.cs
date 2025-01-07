using EventManagementAPI.Domain.Models.Attendance;
using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Interfaces;

public interface IAttendanceRepository
{
    Task<PaginatedList<AttendanceDomainModel>> GetAsync(Guid eventId, int page, int pageSize, CancellationToken cancellation);
    Task<AttendanceDomainModel?> GetAsync(Guid eventId, Guid userId, CancellationToken cancellation);
    Task InsertAsync(AttendanceDomainModel attendanceModel, CancellationToken cancellation = default);
    Task SaveChangesAsync(CancellationToken cancellation = default);
}
