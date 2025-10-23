namespace MASsenger.Core.Entities
{
	public enum MessageType
	{
		DIRECT,
		CHANNEL,
		GROUP,
		BOT
	}
	public class BaseMessage
	{
		public UInt64 Id { get; set; } // id of the message inside of the group/chat/bot
		public MessageType Type { get; set; }
        public BaseUser Sender { get; set; } = null!;
		public BaseChat Destination { get; set; } = null!;
		public DateTime SentTime { get; set; }
		//public BaseMessage? Reply = null;

		// 1st is sent, 2nd is recivied, 3rd one is for seen
		public bool[] SeenMark = new bool[3];
		public ICollection<User> SeenUsers { get; set; } = new List<User>();

		/* i do miss unions */
		public bool IsDeleted;
		public DateTime? DeletionTime = null;
		public BaseUser Deleter { get; set; } = null!;

        public string Text { get; set; } = null!;
    }

    //public class Msg : BaseMsg {
    //	List<File>? attachments = null;
    //	List<MsgText> text;
    //}

    //public class MsgText {
    //	public UInt16 revision;
    //	public string text;
    //	public DateTime mtime;
    //}

    //public class File {
    //	public UInt16 revision;
    //	public String? name = null;
    //	public byte[]? file = null;
    //	public String? hash = null;
    //	public DateTime mtime;
    //}
    //class FwdMsg : BaseMsg {
    //	/* what we do show for the end-user is up to the bussiness layer */
    //	public Msg msg { get; set; }
    //}
}
