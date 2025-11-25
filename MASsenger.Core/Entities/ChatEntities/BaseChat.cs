using MASsenger.Core.Entities.Base;
using MASsenger.Core.Entities.MessageEntities;

namespace MASsenger.Core.Entities.ChatEntities
{
	public class BaseChat : BaseEntity
	{
        /*
		 * there might be insane amount of users/messages
		 * if API calls .len() everytime overhead would be unimagineable
		 * perhaps we could cache them later?
		 */

        // one-to-many relationships
        public ICollection<BaseMessage> Messages { get; set; } = new List<BaseMessage>();
	}
}
