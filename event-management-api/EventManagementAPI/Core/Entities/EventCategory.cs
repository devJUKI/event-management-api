namespace EventManagementAPI.Core.Entities;

public class EventCategory
{
    public Event Event { get; set; } = default!;
    public Guid EventId { get; set; }
    public Category Category { get; set; } = default!;
    public int CategoryId { get; set; }
}
