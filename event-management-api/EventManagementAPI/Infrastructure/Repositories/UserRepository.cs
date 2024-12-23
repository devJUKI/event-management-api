using EventManagementAPI.Domain.Models.Authentication;
using EventManagementAPI.Infrastructure.Persistence;
using EventManagementAPI.Infrastructure.Interfaces;
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

    public async Task<UserDomainModel?> GetAsync(Guid Id, CancellationToken cancellation)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Id == Id, cancellation);

        if (user == null) return null;

        var userModelResult = new UserDomainModel(user);
        return userModelResult;
    }

    public async Task<UserDomainModel?> GetByEmailAsync(string email, CancellationToken cancellation)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Email == email, cancellation);
        
        if (user == null) return null;

        var userModelResult = new UserDomainModel(user);
        return userModelResult;
    }

    public async Task<UserDomainModel?> GetByEmailAndUsernameAsync(string email, string username, CancellationToken cancellation)
    {
        var user = await _dbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u =>
                u.Email == email &&
                u.Username == username,
            cancellation);

        if (user == null) return null;

        var userModelResult = new UserDomainModel(user);
        return userModelResult;
    }

    public async Task InsertAsync(UserDomainModel userModel, CancellationToken cancellation)
    {
        var roles = _dbContext.Roles.Where(r => userModel.Roles.Contains(r.Name)).ToList();

        var user = new User
        {
            Id = userModel.Id,
            Username = userModel.Username,
            Email = userModel.Email,
            HashedPassword = userModel.HashedPassword,
            CreatedAt = userModel.CreatedAt,
            UserRoles = roles.Select(role => new UserRole
            {
                RoleId = role.Id,
                Role = role
            }).ToList()
        };

        await _dbContext.Users.AddAsync(user, cancellation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellation)
    {
        await _dbContext.SaveChangesAsync(cancellation);
    }
}
