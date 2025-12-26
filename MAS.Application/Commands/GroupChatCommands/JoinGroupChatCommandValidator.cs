using FluentValidation;

namespace MAS.Application.Commands.GroupChatCommands;

public class JoinGroupChatCommandValidator : AbstractValidator<JoinGroupChatCommand>
{
    public JoinGroupChatCommandValidator()
    {
        RuleFor(g => g.GroupChatId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("GroupChatId is required.")
                .GreaterThan(0)
                .WithMessage("GroupChatId must be greater than zero.");
    }
}
