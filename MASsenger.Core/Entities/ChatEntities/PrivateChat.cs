using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Core.Entities.ChatEntities
{
	public class PrivateChat : BaseChat
	{
		public BaseUser Starter { get; set; } = null!;
		public BaseUser Receiver { get; set; } = null!;
	}
}
