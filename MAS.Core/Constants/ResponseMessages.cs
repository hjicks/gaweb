using MAS.Core.Enums;

namespace MAS.Core.Constants;

public class ResponseMessages
{
    public static readonly Dictionary<ErrorType, string> Error = new()
    {
        { ErrorType.Exception, "Server error." },
        { ErrorType.Unauthorized, "User not authorized, please login." },
        { ErrorType.InvalidRequestModel, "Request structure is not valid. Check below for more details." },
        { ErrorType.Validation, "Input data is invalid. Check below for more details." },
        { ErrorType.UserNotFound, "User not found in the system." },
        { ErrorType.SessionNotFound, "Session not found in the system." },
        { ErrorType.ChatNotFound, "Chat not found in the system." },
        { ErrorType.MessageNotFound, "Message not found in the system." },
        { ErrorType.UsernameAlreadyExists, "Username is already taken. Please choose another." },
        { ErrorType.GroupnameAlreadyExists, "Groupname is already taken. Please choose another." },
        { ErrorType.ChatAlreadyExists, "Chat already exists in the system." },
        { ErrorType.InvalidCredentials, "Username or password is incorrect." },
        { ErrorType.InvalidOrExpiredRefreshToken, "Your refresh token is invalid or expired." },
        { ErrorType.PermissionDenied, "You are not authorized to perform this action." },
        { ErrorType.ActiveSessionAvailable, "Please log out from your other session first." },
        { ErrorType.ChatIsEmpty, "Chat have no message." },
        { ErrorType.GroupMemberAlreadyJoinedOrIsBanned, "Member is either already joined the group or banned from it." }
    };
}
