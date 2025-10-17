namespace MASsenger
{	
	public class DirectChat : BaseChat {
		public bool IsBot; /*
							* Bots can't communicate with other bots,
							* this indicates *Exactly* one side
							* of communication is a bot
							*/
	}
}
