namespace MAS.Core.Options;

public class TokenOptions
{
    public const string SectionName = "Tokens";
    public AccessToken AccessToken { get; set; } = new AccessToken();
    public RefreshToken RefreshToken { get; set; } = new RefreshToken();
}

public class AccessToken
{
    public string Key { get; set; } = string.Empty;
    public int ExpiryInMins { get; set; }
}

public class RefreshToken
{
    public string Key { get; set; } = string.Empty;
    public int ExpiryInDays { get; set; }
}