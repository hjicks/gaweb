using FluentValidation;

namespace MAS.Application.Commands.GroupChatCommands;

public class PromoteOrDemoteGroupMemberCommandValidator : AbstractValidator<PromoteOrDemoteGroupMemberCommand>
{
    public PromoteOrDemoteGroupMemberCommandValidator()
    {
        RuleFor(g => g.GroupChatId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("GroupChatId is required.")
                .GreaterThan(0)
                .WithMessage("GroupChatId must be greater than zero.");

        RuleFor(g => g.MemberId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("MemberId is required.")
                .GreaterThan(0)
                .WithMessage("MemberId must be greater than zero.");
    }
}
