using MAS.Core.Entities.Base;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.UserEntities;

namespace MAS.Core.Entities.MessageEntities;

public class Message : BaseEntity
{
    public int SenderId { get; set; }
    public int DestinationId { get; set; }
    public string? Text { get; set; } = string.Empty;
    public string? FileName { get; set; } = string.Empty;
    public ulong? FileSize { get; set; }
    public int? FileContentId { get; set; }
    public string? FileContentType { get; set; } = string.Empty; // MIME type

    // navigation properties

    // one-to-one
    public FileContent? FileContent { get; set; }

    // one-to-many
    public User Sender { get; set; } = new User();
    public BaseChat Destination { get; set; } = new BaseChat();
}
