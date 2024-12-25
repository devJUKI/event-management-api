using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Infrastructure.Persistence;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagementAPI.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly EventManagementDbContext _dbContext;

    public EventRepository(EventManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EventDomainModel?> GetAsync(Guid Id, CancellationToken cancellation)
    {
        var @event = await _dbContext.Events
            .SingleOrDefaultAsync(e => e.Id == Id, cancellation);

        if (@event == null) return null;

        var eventModel = new EventDomainModel(@event);
        return eventModel;
    }

    public async Task InsertAsync(EventDomainModel eventModel, CancellationToken cancellation)
    {
        var eventCategories = await _dbContext.Categories
            .Where(c => eventModel.Categories.Contains(c.Name))
            .Select(@event => new EventCategory
            {
                CategoryId = @event.Id,
                EventId = eventModel.Id
            })
            .ToListAsync(cancellation);

        if (eventCategories.Count > eventModel.Categories.Count)
        {
            throw new InvalidDataException("Invalid categories specified.");
        }

        var @event = new Event
        {
            Id = eventModel.Id,
            Title = eventModel.Title,
            Description = eventModel.Description,
            Date = eventModel.Date,
            CreatedById = eventModel.CreatedBy.Id,
            IsPublic = eventModel.IsPublic,
            Location = eventModel.Location,
            EventCategories = eventCategories
        };

        await _dbContext.Events.AddAsync(@event, cancellation);
    }

    public async Task UpdateAsync(EventDomainModel eventModel, CancellationToken cancellation)
    {
        var eventCategories = await _dbContext.Categories
            .Where(c => eventModel.Categories.Contains(c.Name))
            .Select(@event => new EventCategory
            {
                CategoryId = @event.Id,
                EventId = eventModel.Id
            })
            .ToListAsync(cancellation);

        var @event = await _dbContext.Events.FindAsync(eventModel.Id, cancellation);

        if (@event == null)
        {
            throw new InvalidOperationException(nameof(@event));
        }

        @event.Title = eventModel.Title;
        @event.Description = eventModel.Description;
        @event.Date = eventModel.Date;
        @event.Location = eventModel.Location;
        @event.IsPublic = eventModel.IsPublic;
        @event.EventCategories = eventCategories;

        _dbContext.Events.Update(@event);
    }

    public async Task SaveChangesAsync(CancellationToken cancellation)
    {
        await _dbContext.SaveChangesAsync(cancellation);
    }
}
