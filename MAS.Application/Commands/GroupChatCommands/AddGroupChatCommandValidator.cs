using FluentValidation;

namespace MAS.Application.Commands.GroupChatCommands
{
    public class AddGroupChatCommandValidator : AbstractValidator<AddGroupChatCommand>
    {
        public AddGroupChatCommandValidator()
        {
            RuleFor(c => c.GroupChat.Groupname)
                .NotEmpty()
                .WithMessage("A group username is required.")
                .Length(1, 16)
                .WithMessage("A group username must be between 1 character and 16 characters")
                .Matches("^[a-zA-Z0-9_]+$")
                .WithMessage("group username can only contain letters, numbers and underscore.");
        }
    }
}
