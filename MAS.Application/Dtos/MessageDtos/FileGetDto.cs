namespace MAS.Application.Dtos.MessageDtos;

public record FileGetDto
{
    public int MessageId { get; set; }
    public string? Content { get; set; }
}
