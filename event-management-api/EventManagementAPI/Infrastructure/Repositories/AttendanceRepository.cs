using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Infrastructure.Persistence;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Models.Attendance;
using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Domain.Entities;

namespace EventManagementAPI.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly EventManagementDbContext _dbContext;

    public AttendanceRepository(EventManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<AttendanceDomainModel>> GetAsync(Guid eventId, int pageSize, int page, CancellationToken cancellation = default)
    {
        var query = _dbContext.Attendances.Where(a => a.EventId == eventId);

        var totalCount = await query.CountAsync(cancellation);

        var attendances = await query
            .Include(a => a.User)
            .Include(a => a.Event)
            .OrderBy(a => a.UserId)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .Select(a => new AttendanceDomainModel
            {
                User = new UserDomainModel(a.User),
                Event = new EventDomainModel(a.Event),
                Status = a.Status,
                Timestamp = a.Timestamp
            })
            .ToListAsync(cancellation);

        var paginatedModels = new PaginatedList<AttendanceDomainModel>(attendances, page, totalCount, pageSize);

        return paginatedModels;
    }

    public async Task<AttendanceDomainModel?> GetAsync(Guid eventId, Guid userId, CancellationToken cancellation)
    {
        var attendance = await _dbContext.Attendances
            .Include(a => a.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(a => a.Event)
            .ThenInclude(e => e.EventCategories)
            .ThenInclude(ec => ec.Category)
            .SingleOrDefaultAsync(a =>
                a.EventId == eventId &&
                a.UserId == userId,
                cancellation);

        if (attendance == null)
        {
            return null;
        }

        var domainModel = new AttendanceDomainModel
        {
            Event = new EventDomainModel(attendance.Event),
            User = new UserDomainModel(attendance.User),
            Status = attendance.Status,
            Timestamp = attendance.Timestamp,
        };

        return domainModel;
    }

    public async Task InsertAsync(AttendanceDomainModel attendanceModel, CancellationToken cancellation = default)
    {
        var attendance = new Attendance
        {
            EventId = attendanceModel.Event.Id,
            UserId = attendanceModel.User.Id,
            Status = attendanceModel.Status,
            Timestamp = attendanceModel.Timestamp
        };

        await _dbContext.Attendances.AddAsync(attendance, cancellation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellation = default)
    {
        await _dbContext.SaveChangesAsync(cancellation);
    }
}
