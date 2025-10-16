public class Channel
{
    public int Id { get; set; }
    public string Name { get; set; } = "MASsenger Channel";
    public string? Description { get; set; }
    public string? Photo { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPublic { get; set; } = false;
    public int MembersCount { get; set; } = 1;

    // Foreign key relationships
    public string Creator { get; set; } // User foreign key


    // Navigation properties
    public ICollection<Group> Groups { get; set; } = new List<Group>();
    public ICollection<Bot> Bots { get; set; } = new List<Bot>();
}
