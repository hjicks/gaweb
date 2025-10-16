namespace MASsenger
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublic { get; set; } = false;
        public int MembersCount { get; set; } = 1;

        // Foreign key relationships
        public int GroupId { get; set; }
        public Group? Group { get; set; }

        // Navigation properties
        public ICollection<User> Members { get; set; } = new List<User>();
        public ICollection<Bot> Bots { get; set; } = new List<Bot>();
    }
}