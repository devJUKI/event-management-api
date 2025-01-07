using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Domain.Enums;

namespace EventManagementAPI.Domain.Models.Attendance;

public class AttendanceDomainModel
{
    public EventDomainModel Event { get; set; } = default!;
    public UserDomainModel User { get; set; } = default!;
    public AttendanceStatus Status { get; set; }
    public DateTime Timestamp { get; set; }
}
