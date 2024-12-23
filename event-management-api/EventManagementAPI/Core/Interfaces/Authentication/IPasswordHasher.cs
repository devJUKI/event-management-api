namespace EventManagementAPI.Core.Interfaces.Authentication;

public interface IPasswordHasher
{
    public string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
