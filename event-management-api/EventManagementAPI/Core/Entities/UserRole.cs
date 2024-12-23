namespace EventManagementAPI.Core.Entities;

public class UserRole
{
    public Role Role { get; set; } = default!;
    public int RoleId { get; set; }
    public User User { get; set; } = default!;
    public Guid UserId { get; set; }
}
