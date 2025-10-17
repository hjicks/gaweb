namespace MASsenger
{
	public class ChanChat : BaseChat {
		/* if it's null then it's not public */
		public String? Name = null;
		public String? Username = null;
		public String? Description = null;
		public File? ProfilePicture = null;

		/* this shouldn't be null */
		public BaseUser? Owner {set; get;} = null;
		
		/* TODO: admins might need annoations and permissions, like in tg */
		public List<BaseUser> Admins {get; set;}
		public List<BaseUser> Banned;

		/* TODO: fix getters to return opposite value of each other(?) */
		public bool IsGroup { get; }
		public bool IsChannel { get; }
	}
}
