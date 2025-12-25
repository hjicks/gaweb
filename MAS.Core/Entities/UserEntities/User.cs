using MAS.Core.Entities.Base;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Entities.MessageEntities;

namespace MAS.Core.Entities.UserEntities;

public class User : BaseEntity
{
    public string DisplayName { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Bio { get; set; }
    public byte[]? Avatar { get; set; }
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public bool IsVerified { get; set; } = false;
    public bool IsBot { get; set; } = false;
    public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // navigation properties

    // many-to-one
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();

    // many-to-many
    public ICollection<PrivateChat> PrivateChats { get; set; } = new List<PrivateChat>();
    public ICollection<GroupChatUser> GroupChats { get; set; } = new List<GroupChatUser>();
}
