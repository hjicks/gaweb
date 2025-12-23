using System.Text.Json.Serialization;

namespace MAS.ChatBot;

public class LlmResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("output")]
    public List<ResponseMessage> Output { get; set; } = new List<ResponseMessage>();

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; } = new Usage();

    [JsonPropertyName("previous_response_id")]
    public string? PreviousResponseId { get; set; }
}

public class ResponseMessage
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public List<MessageContent> Content { get; set; } = new List<MessageContent>();
}

public class MessageContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

public class Usage
{
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }

    [JsonPropertyName("output_tokens_details")]
    public OutputTokensDetails OutputTokensDetails { get; set; } = new OutputTokensDetails();
}

public class OutputTokensDetails
{
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; set; }
}
