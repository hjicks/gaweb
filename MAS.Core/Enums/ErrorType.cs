namespace MAS.Core.Enums
{
    public enum ErrorType
    {
        Exception,
        Validation,
        NotFound,
        AlreadyExists,
        InvalidRequestModel,
        InvalidCredentials,
        InvalidRefreshToken,
        ExpiredRefreshToken,
        PermissionDenied,
        ActiveSessionAvailable
    }
}
