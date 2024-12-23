namespace EventManagementAPI.Domain.Models.Authentication;

public class LoginDomainModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
