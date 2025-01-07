using EventManagementAPI.Domain.Enums;

namespace EventManagementAPI.Domain.Models.Attendance;

public class CreateAttendanceDomainModel
{
    public Guid EventId { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
}
