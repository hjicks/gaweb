using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Application.Commands.UserCommands
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator() 
        {
            RuleFor(c => c.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("UserId is required.")
                .GreaterThan(0)
                .WithMessage("UserId must be greater than zero.");
        }
    }
}
