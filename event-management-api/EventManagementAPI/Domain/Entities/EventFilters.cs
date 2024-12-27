namespace EventManagementAPI.Domain.Entities;

public class EventFilters
{
    public string? Title { get; set; }
    public string? Location { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public bool? IsPublic { get; set; }
    public string? Category { get; set; }
}
