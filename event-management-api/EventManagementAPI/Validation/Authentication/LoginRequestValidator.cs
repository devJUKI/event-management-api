using EventManagementAPI.ViewModels.Authentication;
using FluentValidation;

namespace EventManagementAPI.Validation.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequestViewModel>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Password).NotEmpty().Must(r => !string.IsNullOrWhiteSpace(r));
    }
}
