namespace EventManagementAPI.ViewModels.EventManagement;

public record EventResponseViewModel(
    Guid Id,
    string Title,
    string Description,
    string Location,
    string Date,
    string Time,
    string CreatedBy,
    bool IsPublic,
    List<string> Categories);
