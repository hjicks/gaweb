namespace MASsenger {
	public class BaseMsg {
		public UInt64 Id { get; set; } // id of the message inside of the group/chat/bot
		public BaseUser sender;
		public BaseChat dst;
		public DateTime sentTime;
		public BaseMsg? reply = null;

		// 1st is sent, 2nd is recivied, 3rd one is for seen
		public bool[] seenMark = new bool[3]; 
		public List<User> seenUsers;

		/* i do miss unions */
		public bool isDeleted;
		public DateTime? deletionTime = null;
		public BaseUser deleter;
	}

	public class Msg : BaseMsg {
		List<File>? attachments = null;
		List<MsgText> text;
	}

	public class MsgText {
		public UInt16 revision;
		public string text;
		public DateTime mtime;
	}

	public class File {
		public UInt16 revision;
		public String? name = null;
		public byte[]? file = null;
		public String? hash = null;
		public DateTime mtime;
	}
	class FwdMsg : BaseMsg {
		/* what we do show for the end-user is up to the bussiness layer */
		public Msg msg { get; set; }
	}
}
