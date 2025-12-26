using FluentValidation;

namespace MAS.Application.Commands.SessionCommands;

public class RefreshSessionCommandValidator : AbstractValidator<RefreshSessionCommand>
{
    public RefreshSessionCommandValidator()
    {
        RuleFor(c => c.TokenDto.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("RefreshToken is required.")
            .Length(44)
            .WithMessage("RefreshToken must be exactly 44 characters long.");
    }
}
