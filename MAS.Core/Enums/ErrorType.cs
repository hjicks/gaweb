namespace MAS.Core.Enums;

public enum ErrorType
{
    Exception,
    Unauthorized,
    InvalidRequestModel,
    Validation,
    UserNotFound,
    SessionNotFound,
    ChatNotFound,
    MessageNotFound,
    UsernameAlreadyExists,
    InvalidCredentials,
    InvalidOrExpiredRefreshToken,
    PermissionDenied,
    ActiveSessionAvailable,
    ChatIsEmpty
}
