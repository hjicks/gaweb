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
    FileContentNotFound,
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
