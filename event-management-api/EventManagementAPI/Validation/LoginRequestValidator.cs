using EventManagementAPI.ViewModels.Authentication;
using FluentValidation;

namespace EventManagementAPI.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequestViewModel>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Password).NotEmpty().MinimumLength(3).Must(r => !string.IsNullOrWhiteSpace(r));
    }
}
