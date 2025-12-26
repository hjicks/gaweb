using FluentValidation;

namespace MAS.Application.Commands.MessageCommands;

public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
{
    public DeleteMessageCommandValidator() 
    {
        RuleFor(c => c.MessageId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("MessageId is required.")
            .GreaterThan(0)
            .WithMessage("MessageId must be greater than zero.");
    }
}
