using MAS.Core.Entities.UserEntities;

namespace MAS.Core.Entities.ChatEntities
{
	public class ChannelChat : BaseChat {
		/* if it's null then it's not public */
		public string? Name { get; set; } = null;
		public string? Username { get; set; } = null;
		public string? Description { get; set; } = null;
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        // many-to-many relationships
        /* TODO: admins might need annoations and permissions, like in tg */
        public ICollection<BaseUser> Admins { get; set; } = new List<BaseUser>();
        public ICollection<BaseUser> Banned { get; set; } = new List<BaseUser>();
        public ICollection<BaseUser> Members { get; set; } = new List<BaseUser>();
    }
}
