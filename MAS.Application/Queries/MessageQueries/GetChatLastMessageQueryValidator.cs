using FluentValidation;

namespace MAS.Application.Queries.MessageQueries;

public class GetChatLastMessageQueryValidator : AbstractValidator<GetChatLastMessageQuery>
{
    public GetChatLastMessageQueryValidator()
    {
        RuleFor(g => g.ChatId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("ChatId is required.")
                .GreaterThan(0)
                .WithMessage("ChatId must be greater than zero.");
    }
}
