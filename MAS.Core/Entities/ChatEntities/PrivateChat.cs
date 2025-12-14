using MAS.Core.Entities.UserEntities;

namespace MAS.Core.Entities.ChatEntities
{
	public class PrivateChat : BaseChat
	{
		public int StarterId { get; set; }
		public BaseUser Starter { get; set; } = null!;
		public int ReceiverId { get; set; }
		public BaseUser Receiver { get; set; } = null!;
	}
}
