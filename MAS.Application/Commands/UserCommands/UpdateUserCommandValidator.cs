using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Application.Commands.UserCommands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c => c.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .GreaterThan(0)
                .WithMessage("UserId must be greater than zero.");

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
                .WithMessage("Username or password is incorrect.");

            RuleFor(c => c.User.Bio)
                .MaximumLength(200)
                .WithMessage("Bio cannot exceed 200 characters.");
        }
    }
}
