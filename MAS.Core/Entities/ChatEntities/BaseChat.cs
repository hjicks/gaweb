using MAS.Core.Entities.Base;
using MAS.Core.Entities.MessageEntities;

namespace MAS.Core.Entities.ChatEntities;

public class BaseChat : BaseEntity
{
    // navigation properties

    // many-to-one
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
