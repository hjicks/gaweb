using FluentValidation;

namespace MAS.Application.Commands.UserCommands;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator() 
    {
        RuleFor(c => c.User.DisplayName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("DisplayName is required.")
            .Length(1, 32)
            .WithMessage("DisplayName must be between 1 and 32 characters.");

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
            .WithMessage("Password must be between 8 and 128 characters.");

        RuleFor(c => c.User.Bio)
            .MaximumLength(200)
            .WithMessage("Bio cannot exceed 200 characters.");

        RuleFor(c => c.User.ClientName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("ClientName is required.")
            .Length(1, 64)
            .WithMessage("ClientName must be between 1 and 64 characters.");

        RuleFor(c => c.User.OS)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("OS is required.")
            .Length(1, 64)
            .WithMessage("OS must be between 1 and 64 characters.");
    }
}
