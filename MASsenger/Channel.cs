public class Channel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public string Creator { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPublic { get; set; }
    public int MembersCount { get; set; }


    // Navigation properties
    public ICollection<Group> Groups { get; set; } = new List<Group>();
    public ICollection<Bot> Bots { get; set; } = new List<Bot>();
}