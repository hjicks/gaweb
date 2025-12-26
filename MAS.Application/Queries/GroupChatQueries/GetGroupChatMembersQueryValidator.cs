using FluentValidation;

namespace MAS.Application.Queries.GroupChatQueries;

public class GetGroupChatMembersQueryValidator : AbstractValidator<GetGroupChatMembersQuery>
{
    public GetGroupChatMembersQueryValidator()
    {
        RuleFor(g => g.GroupChatId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("GroupChatId is required.")
                .GreaterThan(0)
                .WithMessage("GroupChatId must be greater than zero.");
    }
}
