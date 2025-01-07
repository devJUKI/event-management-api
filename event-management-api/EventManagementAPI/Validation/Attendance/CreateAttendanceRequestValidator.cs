using EventManagementAPI.ViewModels.Attendance;
using FluentValidation;

namespace EventManagementAPI.Validation.Attendance;

public class CreateAttendanceRequestValidator : AbstractValidator<CreateAttendanceRequestViewModel>
{
    public CreateAttendanceRequestValidator()
    {
        RuleFor(r => r.Status).NotNull();
        RuleFor(r => r.Status).Must(s => Enum.IsDefined(s!.Value)).WithMessage("Attendance status is invalid.");
    }
}
