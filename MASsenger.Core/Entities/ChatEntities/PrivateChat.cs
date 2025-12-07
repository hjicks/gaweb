using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Core.Entities.ChatEntities
{
	public class PrivateChat : BaseChat
	{
		public int StarterId { get; set; }
		public BaseUser Starter { get; set; } = null!;
		public int ReceiverId { get; set; }
		public BaseUser Receiver { get; set; } = null!;
	}
}
