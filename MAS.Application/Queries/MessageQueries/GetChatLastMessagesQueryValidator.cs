using FluentValidation;

namespace MAS.Application.Queries.MessageQueries;

public class GetChatLastMessagesQueryValidator : AbstractValidator<GetChatLastMessagesQuery>
{
    public GetChatLastMessagesQueryValidator()
    {
        RuleFor(g => g.MessageId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("MessageId is required.")
                .GreaterThan(0)
                .WithMessage("MessageId must be greater than zero.");
    }
}
