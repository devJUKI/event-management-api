using EventManagementAPI.ViewModels.UserManagement;
using FluentValidation;

namespace EventManagementAPI.Validation.UserManagement;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestViewModel>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Username).NotEmpty().MinimumLength(3).Must(r => !string.IsNullOrWhiteSpace(r));
    }
}
