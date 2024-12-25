namespace EventManagementAPI.Core.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public User CreatedBy { get; set; } = default!;
    public Guid CreatedById { get; set; }
    public bool IsPublic { get; set; }
    public List<EventCategory> EventCategories { get; set; } = [];
}
