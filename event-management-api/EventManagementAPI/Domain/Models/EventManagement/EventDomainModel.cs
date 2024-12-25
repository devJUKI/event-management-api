using EventManagementAPI.Domain.Models.Common;
using EventManagementAPI.Domain.Constants;
using EventManagementAPI.Domain.Entities;
using EventManagementAPI.Core.Entities;
using System.Data;
using System.Net;

namespace EventManagementAPI.Domain.Models.EventManagement;

public class EventDomainModel
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }
    public UserDomainModel CreatedBy { get; } = default!;
    public bool IsPublic { get; private set; }
    public List<string> Categories { get; private set; } = [];

    private EventDomainModel(string title, string description, string location, DateTime date, UserDomainModel createdBy, bool isPublic, List<string> categories)
    {
        Title = title;
        Description = description;
        Location = location;
        Date = date;
        CreatedBy = createdBy;
        IsPublic = isPublic;
        Categories = categories;
    }

    public EventDomainModel(Event @event)
    {
        Id = @event.Id;
        Title = @event.Title;
        Description = @event.Description;
        Location = @event.Location;
        CreatedBy = new UserDomainModel(@event.CreatedBy);
        IsPublic = @event.IsPublic;
        Categories = @event.EventCategories
            .Select(r => r.Category.Name)
            .ToList();
    }

    public static Result<EventDomainModel> Create(string? title, string? description, string? location, DateTime? date, UserDomainModel? createdBy, bool? isPublic, List<string>? categories)
    {
        var validationResult = ValidateUserData(title, location, date, createdBy, isPublic, categories);

        if (!validationResult.IsSuccess)
        {
            return Result.Failure<EventDomainModel>(validationResult.Error);
        }

        var @event = new EventDomainModel(title!, description ?? string.Empty, location!, date!.Value, createdBy!, isPublic!.Value, categories!);
        return Result.Success(@event);
    }

    private static Result ValidateUserData(string? title, string? location, DateTime? date, UserDomainModel? createdBy, bool? isPublic, List<string>? categories)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < EventData.MinTitleLength)
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Title is invalid."));
        }

        if (string.IsNullOrWhiteSpace(location) || location.Length < EventData.MinLocationLength)
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Location is invalid."));
        }

        if (!date.HasValue)
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Event date is null."));
        }

        if (createdBy == null)
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Event creator is null."));
        }

        if (!isPublic.HasValue)
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Publicity identifier is null."));
        }

        if (categories == null || categories.Count == 0)
        {
            return Result.Failure(new Error(HttpStatusCode.BadRequest, "Event has to have at least 1 category."));
        }

        return Result.Success();
    }
}
