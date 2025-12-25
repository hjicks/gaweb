using System.Text.Json.Serialization;

namespace MAS.ChatBot;

public class MasError
{
    [JsonPropertyName("error")]
    public ErrorType Error { get; set; }

    [JsonPropertyName("description")]
    public IEnumerable<string> Description { get; set; } = new List<string>();
}

public enum ErrorType
{
    Default,
    Exception,
    Unauthorized,
    InvalidRequestModel,
    Validation,
    SessionNotFound,
    ChatNotFound,
    InvalidCredentials,
    ActiveSessionAvailable
}
