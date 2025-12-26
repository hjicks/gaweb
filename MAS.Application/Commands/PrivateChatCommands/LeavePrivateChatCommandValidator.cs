using FluentValidation;
using MAS.Application.Commands.GroupChatCommands;

namespace MAS.Application.Commands.PrivateChatCommands;

public class LeavePrivateChatCommandValidator : AbstractValidator<LeavePrivateChatCommand>
{
    public LeavePrivateChatCommandValidator()
    {
        RuleFor(c => c.PrivateChatId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("PrivateChatId is required.")
            .GreaterThan(0)
            .WithMessage("PrivateChatId must be greater than zero.");
    }
}
