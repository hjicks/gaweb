namespace MASsenger
{
    public class User : BaseUser
    {
        // many-to-one relationships
        public ICollection<Bot> BotsOwned { get; set; } = new List<Bot>();

        // Many-to-many relationships
        public ICollection<Bot> Bots { get; set; } = new List<Bot>();
		public ICollection<BaseUser> Blocked { get; set; } = new List<BaseUser>();
    }
}
