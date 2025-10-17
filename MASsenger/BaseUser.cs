namespace MASsenger
{
    public class BaseUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; } /* acts also as bio */
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; } = false;


		// one-to-many relationship
		public ICollection<Group> GroupsOwned  = new List<Group>();
		public ICollection<Channel> ChannelsOwned  = new List<Channel>();

		// Many-to-many relationships
		public ICollection<Channel> Channels = new List<Channel>();
		public ICollection<Group> Groups  = new List<Group>();
    }
}
