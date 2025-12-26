using FluentValidation;

namespace MAS.Application.Commands.UserCommands;

public class UpdateUserLastSeenCommandValidator : AbstractValidator<UpdateUserLastSeenCommand>
{
    public UpdateUserLastSeenCommandValidator()
    {
        RuleFor(c => c.User.LastSeenAt)
            .Cascade(CascadeMode.Stop)
            .Must(c => c.Kind == DateTimeKind.Utc)
            .WithMessage("LastSeenAt must be in UTC.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("LastSeenAt cannot be in the future.");
    }
}
