namespace EventManagementAPI.ViewModels.Attendance;

public record AttendanceResponseViewModel(
    Guid EventId,
    Guid UserId,
    string Status,
    DateTime Timestamp);