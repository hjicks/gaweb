using MAS.Core.Enums;
using System.Collections.Immutable;

namespace MAS.Core.Constants;

public class ResponseMessages
{
    public static ImmutableDictionary<ErrorType, string> Error => MutableError.ToImmutableDictionary();

    private static readonly Dictionary<ErrorType, string> MutableError = new()
    {
        { ErrorType.Exception, "Server error." },
        { ErrorType.Unauthorized, "User not authorized, please login." },
        { ErrorType.InvalidRequestModel, "Request structure is not valid. Check below for more details." },
        { ErrorType.Validation, "Input data is invalid. Check below for more details." },
        { ErrorType.UserNotFound, "User not found in the system." },
        { ErrorType.SessionNotFound, "Session not found in the system." },
        { ErrorType.ChatNotFound, "Chat not found in the system." },
        { ErrorType.MemberNotFound, "Member not found in the chat." },
        { ErrorType.MessageNotFound, "Message not found in the system." },
        { ErrorType.FileContentNotFound, "There is no file content for the provided message." },
        { ErrorType.UsernameAlreadyExists, "Username is already taken. Please choose another." },
        { ErrorType.GroupnameAlreadyExists, "Groupname is already taken. Please choose another." },
        { ErrorType.ChatAlreadyExists, "Chat already exists in the system." },
        { ErrorType.InvalidCredentials, "Username or password is incorrect." },
        { ErrorType.InvalidOrExpiredRefreshToken, "Your refresh token is invalid or expired." },
        { ErrorType.PermissionDenied, "You are not authorized to perform this action." },
        { ErrorType.ActiveSessionAvailable, "Please log out from your other session first." },
        { ErrorType.ChatIsEmpty, "Chat have no message." },
        { ErrorType.MemberAlreadyJoinedOrIsBanned, "Member is either already joined the group or banned from it." },
        { ErrorType.UnableToDecodeFileContent, "Unable to decode file content. The content is either invalid or larger than 50 MB." },
        { ErrorType.AvatarIsNotValid, "Avatar is not a valid image. Send a valid image between 1 KB and 2 MB." }
    };

    public static ImmutableDictionary<SuccessType, string> Success => MutableSuccess.ToImmutableDictionary();

    private static readonly Dictionary<SuccessType, string> MutableSuccess = new()
    {
        { SuccessType.DeleteSuccessful, "Resource deleted successfully." },
        { SuccessType.LeaveSuccessful, "User left the chat successfully." },
        { SuccessType.LogoutSuccessful, "Log out successful." }
    };
}
