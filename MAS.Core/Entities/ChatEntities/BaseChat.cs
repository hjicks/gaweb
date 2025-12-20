using MAS.Core.Entities.Base;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Enums;

namespace MAS.Core.Entities.ChatEntities;

public abstract class BaseChat : BaseEntity
{
    public ChatType Type { get; private set; }

    // navigation properties

    // many-to-one
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
