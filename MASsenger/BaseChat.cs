namespace MASsenger {
	/* there ought to be better ways of doing tagged unions */
	public enum ChatType {
		DIRECT,
		CHAN,
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
		
        // Many-to-many relationships
        public ICollection<BaseUser> Members { get; set; } = new List<BaseUser>();
		public ICollection<BaseMsg> Messages { get; set; } = new List<BaseMsg>();
	}	
}
