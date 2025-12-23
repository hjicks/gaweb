using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Application.Commands.MessageCommands
{
    public class AddMessageCommandValidator : AbstractValidator<AddMessageCommand>
    {
        public AddMessageCommandValidator() 
        {
            RuleFor(c => c.Message.DestinationId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("DestinationId is required.")
                .GreaterThan(0)
                .WithMessage("DestinationId must be greater than zero.");

            RuleFor(c => c.Message.FileName)
                .MaximumLength(32)
                .WithMessage("FileName cannot exceed 32 characters.");

            RuleFor(c => c.Message.FileSize)
                .GreaterThan(0UL)
                .WithMessage("FileSize must be greater than zero.");

            RuleFor(c => c.Message.FileContentType)
                .Length(5,32)
                .WithMessage("FileContentType must be between 5 and 32 characters.");
        }
    }
}
