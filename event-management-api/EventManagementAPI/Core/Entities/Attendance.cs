using EventManagementAPI.Domain.Enums;

namespace EventManagementAPI.Core.Entities;

public class Attendance
{
    public Event Event { get; set; } = default!;
    public Guid EventId { get; set; }
    public User User { get; set; } = default!;
    public Guid UserId { get; set; }
    public AttendanceStatus Status { get; set; }
    public DateTime Timestamp { get; set; }
}
