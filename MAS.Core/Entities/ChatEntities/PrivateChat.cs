using MAS.Core.Entities.UserEntities;

namespace MAS.Core.Entities.ChatEntities;

public class PrivateChat : BaseChat
{
    // navigation properties

    // many-to-many
    public ICollection<User> Members { get; set; } = new List<User>();
}
