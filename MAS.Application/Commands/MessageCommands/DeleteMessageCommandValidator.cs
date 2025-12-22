using FluentValidation;
using MAS.Application.Commands.SessionCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Application.Commands.MessageCommands
{
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
}
