namespace MASsenger.Core.Entities
{
	public class ChannelGroupChat : BaseChat {
		/* if it's null then it's not public */
		public String? Name { get; set; } = null;
		public String? Username { get; set; } = null;
		public String? Description { get; set; } = null;
        //public File? ProfilePicture { get; set; } = null;

        // optional one-to-one relationship
        public ChannelGroupChat? LinkedChannelGroup { get; set; } = null;

        // required one-to-many relationship
        public User Owner { get; set; } = null!;

        /* TODO: admins might need annoations and permissions, like in tg */
        public ICollection<BaseUser> Admins { get; set; } = new List<BaseUser>();
        public ICollection<BaseUser> Banned { get; set; } = new List<BaseUser>();

        // many-to-many relationships
        public ICollection<BaseUser> Members { get; set; } = new List<BaseUser>();
    }
}
