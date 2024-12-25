namespace EventManagementAPI.ViewModels.EventManagement;

public class CreateEventRequestViewModel
{
    public string? Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime? Date { get; set; }
    public Guid? CreatedById { get; set; }
    public bool? IsPublic { get; set; }
    public List<int>? CategoryIds { get; set; }
}
