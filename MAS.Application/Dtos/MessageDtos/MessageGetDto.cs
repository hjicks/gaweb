namespace MAS.Application.Dtos.MessageDtos;

public record MessageGetDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public int DestinationId { get; set; }
    public string? Text { get; set; } = string.Empty;
    public string? FileName { get; set; } = string.Empty;
    public ulong? FileSize { get; set; }
    public string? FileContentType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
