using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Application.Commands.SessionCommands
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator() 
        {
            RuleFor(c => c.SessionId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("SessionId is required.")
                .GreaterThan(0)
                .WithMessage("SessionId must be greater than zero.");
        }
    }
}
