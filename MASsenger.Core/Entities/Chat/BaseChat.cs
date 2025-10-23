using MASsenger.Core.Enums;

namespace MASsenger.Core.Entities 
{
	public class BaseChat {
		public UInt64 Id { get; set; }

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
