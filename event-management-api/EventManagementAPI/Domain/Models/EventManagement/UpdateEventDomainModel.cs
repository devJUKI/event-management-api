namespace EventManagementAPI.Domain.Models.EventManagement;

public class UpdateEventDomainModel
{
    public Guid EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsPublic { get; set; }
    public List<int> CategoryIds { get; set; } = [];
}
