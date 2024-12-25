using EventManagementAPI.ViewModels.Authentication;
using EventManagementAPI.Domain.Constants;
using FluentValidation;

namespace EventManagementAPI.Validation.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestViewModel>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Username).NotEmpty().MinimumLength(UserData.MinUsernameLength).Must(r => !string.IsNullOrWhiteSpace(r));
        RuleFor(r => r.Password).NotEmpty().MinimumLength(UserData.MinPasswordLength).Must(r => !string.IsNullOrWhiteSpace(r));
    }
}
