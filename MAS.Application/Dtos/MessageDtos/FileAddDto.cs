namespace MAS.Application.Dtos.MessageDtos
{
    public record FileAddDto
    {
        public int DestinationId { get; set; }
        public string? Text { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public ulong FileSize { get; set; }
        public byte[] FileContent { get; set; } = Array.Empty<byte>();
        public string FileContentType { get; set; } = string.Empty; // MIME type
    }
}
