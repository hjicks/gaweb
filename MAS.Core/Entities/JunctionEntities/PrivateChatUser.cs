namespace MAS.Core.Entities.JunctionEntities
{
    public class PrivateChatUser
    {
        // the name of these foreign keys helps ef understand that this is a junction
        public int UserId { get; set; }
        public int PrivateChatId { get; set; }
    }
}
