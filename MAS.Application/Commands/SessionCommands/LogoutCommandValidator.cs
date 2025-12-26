using FluentValidation;

namespace MAS.Application.Commands.SessionCommands;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator() 
    {
        RuleFor(s => s.TokenDto.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("RefreshToken is required.")
            .Length(44)
            .WithMessage("RefreshToken must be exactly 44 characters long.");
    }
}
