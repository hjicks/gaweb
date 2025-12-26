using FluentValidation;

namespace MAS.Application.Commands.GroupChatCommands;

public class DeleteGroupChatCommandValidator : AbstractValidator<DeleteGroupChatCommand>
{
    public DeleteGroupChatCommandValidator()
    {
        RuleFor(g => g.GroupChatId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("GroupChatId is required.")
                .GreaterThan(0)
                .WithMessage("GroupChatId must be greater than zero.");
    }
}
