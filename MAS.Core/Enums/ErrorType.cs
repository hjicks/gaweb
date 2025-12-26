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
    MemberNotFound,
    MessageNotFound,
    UsernameAlreadyExists,
    GroupnameAlreadyExists,
    ChatAlreadyExists,
    InvalidCredentials,
    InvalidOrExpiredRefreshToken,
    PermissionDenied,
    ActiveSessionAvailable,
    ChatIsEmpty,
    MemberAlreadyJoinedOrIsBanned,
    MemberIsBanned,
    UnableToDecodeFileContent,
    AvatarIsNotValid
}
