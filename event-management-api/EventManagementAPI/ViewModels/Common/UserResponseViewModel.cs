namespace EventManagementAPI.ViewModels.Common;

public record UserResponseViewModel(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAd,
    List<string> Roles);