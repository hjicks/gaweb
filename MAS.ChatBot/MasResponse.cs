using System.Text.Json.Serialization;

namespace MAS.ChatBot;

public class MasResponse
{
    [JsonPropertyName("response")]
    public SessionRefreshResponse Response { get; set; } = new SessionRefreshResponse();
}

public class SessionRefreshResponse
{
    [JsonPropertyName("jwt")]
    public string Jwt { get; set; } = string.Empty;

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}
