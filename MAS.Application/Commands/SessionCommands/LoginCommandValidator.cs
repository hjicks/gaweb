using FluentValidation;

namespace MAS.Application.Commands.SessionCommands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(c => c.User.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Username is required.")
                .Length(5, 32)
                .WithMessage("Username must be between 5 and 32 characters.")
                .Matches("^[a-zA-Z0-9_]+$")
                .WithMessage("Username can only contain letters, numbers and underscore.");

            RuleFor(c => c.User.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Length(8, 128)
                .WithMessage("Username or password is incorrect.");
        }
    }
}
