using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Application.Commands.PrivateChatCommands
{
    public class DeletePrivateChatCommandValidator : AbstractValidator<DeletePrivateChatCommand>
    {
        public DeletePrivateChatCommandValidator() 
        {
            RuleFor(c => c.PrivateChatId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("PrivateChatId is required.")
                .GreaterThan(0)
                .WithMessage("PrivateChatId must be greater than zero.");
        }
    }
}
