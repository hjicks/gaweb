using FluentValidation;

namespace MAS.Application.Commands.ChannelChatCommands
{
    public class AddChannelChatCommandValidator : AbstractValidator<AddChannelChatCommand>
    {
        public AddChannelChatCommandValidator()
        {
            RuleFor(c => c.channelChat.Username)
                .NotEmpty()
                .WithMessage("A channel username is required.")
                .Length(1, 16)
                .WithMessage("A channel username must be between 1 character and 16 characters")
                .Matches("^[a-zA-Z0-9_]+$")
                .WithMessage("Channel username can only contain letters, numbers and underscore.");
        }
    }
}
