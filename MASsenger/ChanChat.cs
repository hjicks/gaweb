namespace MASsenger
{
	public class ChanChat : BaseChat {
		/* if it's null then it's not public */
		public String? Name { get; set; } = null;
		public String? Username { get; set; } = null;
		public String? Description { get; set; } = null;
		public File? ProfilePicture { get; set; } = null;

		/* TODO: this shouldn't be null */
		public BaseUser? Owner { set; get; } = null;
		
		/* TODO: admins might need annoations and permissions, like in tg */
		public List<BaseUser> Admins { get; set; }
		public List<BaseUser> Banned { get; set; }

		/* TODO: fix getters to return opposite value of each other(?) */
		public bool IsGroup { get; }
		public bool IsChannel { get; }
	}
}
