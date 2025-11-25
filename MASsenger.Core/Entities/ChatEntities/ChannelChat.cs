using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Core.Entities.ChatEntities
{
	public class ChannelChat : BaseChat {
		/* if it's null then it's not public */
		public string? Name { get; set; } = null;
		public string? Username { get; set; } = null;
		public string? Description { get; set; } = null;

        // required one-to-many relationship
        public User Owner { get; set; } = null!;

        // many-to-many relationships
        /* TODO: admins might need annoations and permissions, like in tg */
        public ICollection<BaseUser> Admins { get; set; } = new List<BaseUser>();
        public ICollection<BaseUser> Banned { get; set; } = new List<BaseUser>();
        public ICollection<BaseUser> Members { get; set; } = new List<BaseUser>();
    }
}
