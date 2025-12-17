namespace MAS.Core.Entities.JoinEntities;

// this class maps to implicit join entity and is only for representational purposes
public class PrivateChatUser
{
    // the name of these foreign keys helps ef understand that this is a join entity
    public int UserId { get; set; }
    public int PrivateChatId { get; set; }
}
