namespace EventManagementAPI.Core.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<EventCategory> EventCategories { get; set; } = [];
}
