namespace EventManagementAPI.Domain.Interfaces;

public interface IPasswordHasher
{
    public string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
