namespace EventManagementAPI.ViewModels.Attendance;

public record UserAttendanceResponseViewModel(
    Guid UserId,
    string Username,
    string Status,
    DateTime Timestamp);