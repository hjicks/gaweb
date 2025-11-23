namespace MASsenger.Core.Entities
{
	public class PrivateChat : BaseChat
	{
		public BaseUser Starter { get; set; } = null!;
		public BaseUser Receiver { get; set; } = null!;
	}
}
