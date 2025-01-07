using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Models.Attendance;
using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Services;

public class AttendanceInfrastructureService : IAttendanceInfrastructureService
{
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceInfrastructureService(IAttendanceRepository attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }

    public Task<PaginatedList<AttendanceDomainModel>> GetAttendancesAsync(Guid eventId, int pageSize, int page, CancellationToken cancellation = default)
    {
        return _attendanceRepository.GetAsync(eventId, pageSize, page, cancellation);
    }

    public Task<AttendanceDomainModel?> GetAttendanceAsync(Guid eventId, Guid userId, CancellationToken cancellation = default)
    {
        return _attendanceRepository.GetAsync(eventId, userId, cancellation);
    }

    public Task CreateAttendanceAsync(AttendanceDomainModel attendanceModel, CancellationToken cancellation = default)
    {
        return _attendanceRepository.InsertAsync(attendanceModel, cancellation);
    }

    public Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        return _attendanceRepository.SaveChangesAsync(cancellation);
    }
}
