namespace EventManagementAPI.ViewModels.EventManagement;

public record EventResponseViewModel(
    Guid Id,
    string Title,
    string Description,
    string Location,
    DateTime Date,
    string CreatedBy,
    bool IsPublic,
    List<string> Categories);
