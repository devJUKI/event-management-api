using EventManagementAPI.Infrastructure.Persistence;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EventManagementDbContext _dbContext;

    public UserRepository(EventManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDomainModel?> GetAsync(Guid Id, CancellationToken cancellation = default)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Id == Id, cancellation);

        if (user == null) return null;

        var userModel = new UserDomainModel(user);
        return userModel;
    }

    public async Task<UserDomainModel?> GetByEmailAsync(string email, CancellationToken cancellation = default)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Email == email, cancellation);
        
        if (user == null) return null;

        var userModel = new UserDomainModel(user);
        return userModel;
    }

    public async Task<bool> ExistsByEmailOrUsernameAsync(string email, string username, CancellationToken cancellation = default)
    {
        var exists = await _dbContext.Users
            .AnyAsync(u =>
                u.Email == email ||
                u.Username == username,
            cancellation);

        return exists;
    }

    public async Task InsertAsync(UserDomainModel userModel, CancellationToken cancellation = default)
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

    public async Task UpdateAsync(UserDomainModel userModel, CancellationToken cancellation = default)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Id == userModel.Id, cancellation);

        if (user == null)
        {
            throw new InvalidOperationException(nameof(user));
        }

        user.Username = userModel.Username;
        user.Email = userModel.Email;
        user.HashedPassword = userModel.HashedPassword;
        user.CreatedAt = userModel.CreatedAt;

        var existingRoles = user.UserRoles.Select(ec => ec.Role.Name).ToList();
        var rolesToAdd = userModel.Roles.Except(existingRoles).ToList();
        var rolesToRemove = existingRoles.Except(userModel.Roles).ToList();

        user.UserRoles = user.UserRoles
            .Where(ec => !rolesToRemove.Contains(ec.Role.Name))
            .ToList();

        foreach (var roleName in rolesToAdd)
        {
            var role = await _dbContext.Roles
                .SingleOrDefaultAsync(c => c.Name == roleName, cancellation);

            if (role == null)
            {
                throw new InvalidOperationException(nameof(role));
            }

            var userRole = new UserRole
            {
                RoleId = role.Id,
                UserId = user.Id
            };

            user.UserRoles.Add(userRole);
        }

        _dbContext.Users.Update(user);
    }

    public async Task SaveChangesAsync(CancellationToken cancellation)
    {
        await _dbContext.SaveChangesAsync(cancellation);
    }
}
