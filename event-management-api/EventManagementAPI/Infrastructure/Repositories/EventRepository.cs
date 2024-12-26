using EventManagementAPI.Domain.Models.EventManagement;
using EventManagementAPI.Infrastructure.Persistence;
using EventManagementAPI.Infrastructure.Interfaces;
using EventManagementAPI.Domain.Models.Common;
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

    public async Task<PaginatedList<EventDomainModel>> GetAsync(int pageSize, int page, CancellationToken cancellation)
    {
        var totalCount = await _dbContext.Events.CountAsync();

        var events = await _dbContext.Events
            .Include(e => e.CreatedBy)
            .OrderBy(e => e.Id)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .Select(e => new EventDomainModel(e))
            .ToListAsync(cancellation);

        var paginatedModels = new PaginatedList<EventDomainModel>(events, page, totalCount, pageSize);

        return paginatedModels;
    }

    public async Task<EventDomainModel?> GetAsync(Guid Id, CancellationToken cancellation)
    {
        var @event = await _dbContext.Events
            .Include(e => e.CreatedBy)
            .SingleOrDefaultAsync(e => e.Id == Id, cancellation);

        if (@event == null) return null;

        var eventModel = new EventDomainModel(@event);
        return eventModel;
    }

    public async Task InsertAsync(EventDomainModel eventModel, CancellationToken cancellation)
    {
        var eventCategories = await _dbContext.Categories
            .Where(c => eventModel.Categories.Contains(c.Name))
            .Select(c => new EventCategory
            {
                CategoryId = c.Id,
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
        var @event = await _dbContext.Events
            .Include(e => e.EventCategories)
            .ThenInclude(ec => ec.Category)
            .SingleOrDefaultAsync(e => e.Id == eventModel.Id, cancellation);

        if (@event == null)
        {
            throw new InvalidOperationException(nameof(@event));
        }

        @event.Title = eventModel.Title;
        @event.Description = eventModel.Description;
        @event.Date = eventModel.Date;
        @event.Location = eventModel.Location;
        @event.IsPublic = eventModel.IsPublic;

        var existingCategories = @event.EventCategories.Select(ec => ec.Category.Name).ToList();
        var categoriesToAdd = eventModel.Categories.Except(existingCategories).ToList();
        var categoriesToRemove = existingCategories.Except(eventModel.Categories).ToList();

        @event.EventCategories = @event.EventCategories
            .Where(ec => !categoriesToRemove.Contains(ec.Category.Name))
            .ToList();

        foreach (var categoryName in categoriesToAdd)
        {
            var category = await _dbContext.Categories
                .SingleOrDefaultAsync(c => c.Name == categoryName, cancellation);

            if (category == null)
            {
                throw new InvalidOperationException(nameof(category));
            }

            var eventCategory = new EventCategory
            {
                CategoryId = category.Id,
                EventId = @event.Id
            };

            @event.EventCategories.Add(eventCategory);
        }

        _dbContext.Events.Update(@event);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellation)
    {
        var @event = await _dbContext.Events.FindAsync(id, cancellation);

        if (@event == null)
        {
            return;
        }

        _dbContext.Events.Remove(@event);
    }

    public async Task SaveChangesAsync(CancellationToken cancellation)
    {
        await _dbContext.SaveChangesAsync(cancellation);
    }
}
