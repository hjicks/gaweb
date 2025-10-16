namespace MASsenger
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Optional one-to-one relationship with channel
        public Channel? Channel { get; set; }

        // Many-to-many relationships
        public ICollection<User> Members { get; set; } = new List<User>();
        public ICollection<Bot> Bots { get; set; } = new List<Bot>();
    }
}