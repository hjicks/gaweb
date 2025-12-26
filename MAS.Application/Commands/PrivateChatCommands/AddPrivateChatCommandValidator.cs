using FluentValidation;

namespace MAS.Application.Commands.PrivateChatCommands;

public class AddPrivateChatCommandValidator : AbstractValidator<AddPrivateChatCommand>
{
    public AddPrivateChatCommandValidator() 
    {
        RuleFor(c => c.ReceiverId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ReceiverId is required.")
            .GreaterThan(0)
            .WithMessage("ReceiverId must be greater than zero.");
    }
}
