namespace MASsenger.Core.Entities 
{
	/* there ought to be better ways of doing tagged unions */
	public enum ChatType {
		DIRECT,
		CHANNEL,
		GROUP,
		BOT
	}

	public class BaseChat {
		public UInt64 Id { get; set; }

		public ChatType Type;
		public DateTime CreationTime;

        /*
		 * there might be insane amount of users/messages
		 * if API calls .len() everytime overhead would be unimagineable
		 * perhaps we could cache them later?
		 */

        // one-to-many relationships
        public ICollection<BaseMessage> Messages { get; set; } = new List<BaseMessage>();
	}
}
