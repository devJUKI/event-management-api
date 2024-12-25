using EventManagementAPI.Infrastructure.Persistence;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Domain.Models.Common;

namespace EventManagementAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EventManagementDbContext _dbContext;

    public UserRepository(EventManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDomainModel?> GetAsync(Guid Id, CancellationToken cancellation)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Id == Id, cancellation);

        if (user == null) return null;

        var userModel = new UserDomainModel(user);
        return userModel;
    }

    public async Task<UserDomainModel?> GetByEmailAsync(string email, CancellationToken cancellation)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Email == email, cancellation);
        
        if (user == null) return null;

        var userModel = new UserDomainModel(user);
        return userModel;
    }

    public async Task<bool> ExistsByEmailOrUsernameAsync(string email, string username, CancellationToken cancellation)
    {
        var exists = await _dbContext.Users
            .AnyAsync(u =>
                u.Email == email ||
                u.Username == username,
            cancellation);

        return exists;
    }

    public async Task InsertAsync(UserDomainModel userModel, CancellationToken cancellation)
    {
        var userRoles = await _dbContext.Roles
            .Where(r => userModel.Roles.Contains(r.Name))
            .Select(role => new UserRole
            {
                RoleId = role.Id,
                UserId = userModel.Id
            })
            .ToListAsync(cancellation);

        if (userRoles.Count > userModel.Roles.Count)
        {
            throw new InvalidDataException("Invalid roles specified.");
        }

        var user = new User
        {
            Id = userModel.Id,
            Username = userModel.Username,
            Email = userModel.Email,
            HashedPassword = userModel.HashedPassword,
            CreatedAt = userModel.CreatedAt,
            UserRoles = userRoles
        };

        await _dbContext.Users.AddAsync(user, cancellation);
    }

    public async Task UpdateAsync(UserDomainModel userModel, CancellationToken cancellation)
    {
        var userRoles = await _dbContext.Roles
            .Where(r => userModel.Roles.Contains(r.Name))
            .Select(role => new UserRole
            {
                RoleId = role.Id,
                Role = role
            })
            .ToListAsync(cancellation);

        var user = await _dbContext.Users.FindAsync(userModel.Id, cancellation);

        if (user == null)
        {
            throw new InvalidOperationException(nameof(user));
        }

        user.Username = userModel.Username;
        user.Email = userModel.Email;
        user.HashedPassword = userModel.HashedPassword;
        user.CreatedAt = userModel.CreatedAt;

        _dbContext.Users.Update(user);
    }

    public async Task SaveChangesAsync(CancellationToken cancellation)
    {
        await _dbContext.SaveChangesAsync(cancellation);
    }
}
