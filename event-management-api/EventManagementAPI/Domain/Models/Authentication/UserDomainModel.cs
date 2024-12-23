using EventManagementAPI.Core.Entities;
using EventManagementAPI.Domain.Constants;
using EventManagementAPI.Domain.Entities;
using System.Net;
using System.Text.RegularExpressions;

namespace EventManagementAPI.Domain.Models.Authentication;

public class UserDomainModel
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Username { get; }
    public string Email { get; }
    public string HashedPassword { get; }
    public DateTime CreatedAt { get; }
    public List<string> Roles { get; }

    private static readonly Regex _emailRegex = new("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");

    private UserDomainModel(string username, string email, string hashedPassword, DateTime createdAt, List<string> roles)
    {
        Username = username;
        Email = email;
        HashedPassword = hashedPassword;
        CreatedAt = createdAt;
        Roles = roles.Distinct().ToList();
    }

    public UserDomainModel(User user)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        HashedPassword = user.HashedPassword;
        CreatedAt = user.CreatedAt;
        Roles = user.UserRoles
            .Select(r => r.Role.Name)
            .ToList();
    }

    public static Result<UserDomainModel> Create(string? username, string? email, string? hashedPassword, DateTime? createdAt, List<string> roles)
    {
        var validationResult = ValidateUserData(username, email, hashedPassword, createdAt);
        if (!validationResult.IsSuccess)
        {
            return Result.Failure<UserDomainModel>(validationResult.Error);
        }

        var user = new UserDomainModel(username!, email!, hashedPassword!, createdAt!.Value, roles);
        return Result.Success(user);
    }

    private static Result ValidateUserData(string? username, string? email, string? hashedPassword, DateTime? createdAt)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < UserData.MinUsernameLength)
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Username is invalid"));
        }

        if (string.IsNullOrWhiteSpace(email) || !_emailRegex.IsMatch(email))
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Email is invalid"));
        }

        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Hashed password is invalid"));
        }

        if (!createdAt.HasValue)
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Creation date is null"));
        }

        return Result.Success();
    }
}
