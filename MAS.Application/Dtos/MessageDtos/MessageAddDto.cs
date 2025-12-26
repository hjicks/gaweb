namespace MAS.Application.Dtos.MessageDtos;

public record MessageAddDto
{
    public int DestinationId { get; set; }
    public string? Text { get; set; }
    public string? FileName { get; set; }
    public ulong? FileSize { get; set; }
    public string? Content { get; set; }
    public string? FileContentType { get; set; }
}
