using EventManagementAPI.ViewModels.Common;

namespace EventManagementAPI.ViewModels.Authentication;

public record AuthResponseViewModel(
    UserResponseViewModel User,
    string Token);
