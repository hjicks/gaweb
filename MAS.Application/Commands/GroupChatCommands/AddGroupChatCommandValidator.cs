using FluentValidation;

namespace MAS.Application.Commands.GroupChatCommands;

public class AddGroupChatCommandValidator : AbstractValidator<AddGroupChatCommand>
{
    public AddGroupChatCommandValidator()
    {
        RuleFor(g => g.GroupChat.Groupname)
            .Length(5, 16)
            .WithMessage("Groupname must be between 5 and 16 characters.")
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage("Groupname can only contain letters, numbers and underscore.");
    }
}
