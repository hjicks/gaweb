using FluentValidation;

namespace MAS.Application.Commands.GroupChatCommands;

public class UpdateGroupChatCommandValidator : AbstractValidator<UpdateGroupChatCommand>
{
    public UpdateGroupChatCommandValidator()
    {
        RuleFor(g => g.GroupChat.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Id is required.")
                .GreaterThan(0)
                .WithMessage("Id must be greater than zero.");

        RuleFor(g => g.GroupChat.DisplayName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("DisplayName is required.")
            .Length(1, 32)
            .WithMessage("DisplayName must be between 1 and 32 characters.");

        RuleFor(g => g.GroupChat.Groupname)
            .Cascade(CascadeMode.Stop)
            .Length(5, 16)
            .WithMessage("Groupname must be between 5 and 16 characters.")
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage("Groupname can only contain letters, numbers and underscore.");

        RuleFor(g => g.GroupChat.Description)
            .MaximumLength(200)
            .WithMessage("Description cannot exceed 200 characters.");

        RuleFor(g => g.GroupChat.MsgPermissionType)
            .IsInEnum()
            .WithMessage("Invalid message permission type.");
    }
}
