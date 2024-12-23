using EventManagementAPI.ViewModels.Authentication;
using FluentValidation;

namespace EventManagementAPI.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestViewModel>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Username).NotEmpty().MinimumLength(3).Must(r => !string.IsNullOrWhiteSpace(r));
        RuleFor(r => r.Password).NotEmpty().MinimumLength(3).Must(r => !string.IsNullOrWhiteSpace(r));
    }
}
