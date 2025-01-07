using EventManagementAPI.Domain.Enums;

namespace EventManagementAPI.ViewModels.Attendance;

public class CreateAttendanceRequestViewModel
{
    public AttendanceStatus? Status { get; set; }
}
