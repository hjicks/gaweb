using MAS.Core.Entities.Base;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.UserEntities;

namespace MAS.Core.Entities.MessageEntities;

public class Message : BaseEntity
{
    public int SenderId { get; set; }
    public int DestinationId { get; set; }
    public string? Text { get; set; }
    public string? FileName { get; set; }
    public ulong? FileSize { get; set; }
    public string? FileContentType { get; set; } // MIME type

    // navigation properties

    // one-to-one
    public FileContent? FileContent { get; set; }

    // one-to-many
    public User Sender { get; set; } = new User();
    public BaseChat Destination { get; set; } = null!;
}
