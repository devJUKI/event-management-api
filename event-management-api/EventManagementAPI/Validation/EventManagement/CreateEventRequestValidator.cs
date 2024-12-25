using EventManagementAPI.ViewModels.EventManagement;
using EventManagementAPI.Domain.Constants;
using FluentValidation;

namespace EventManagementAPI.Validation.EventManagement;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequestViewModel>
{
    public CreateEventRequestValidator()
    {
        RuleFor(r => r.Title).NotEmpty().MinimumLength(5);
        RuleFor(r => r.Location).NotEmpty().MinimumLength(5);
        RuleFor(r => r.Date).NotNull();
        RuleFor(r => r.CreatedById).NotNull();
        RuleFor(r => r.IsPublic).NotNull();
        RuleFor(r => r.CategoryIds)
            .NotEmpty()
            .Must(i => i!.Count <= EventData.MaxCategoryCount)
            .WithMessage($"Event must not exceed {EventData.MaxCategoryCount} categories");
    }
}
