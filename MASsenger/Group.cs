public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = "MASsenger Group";
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // Foreign key relationships
    public Channel? Channel { get; set; }
    public string Creator { get; set; } // User foreign key

    // Navigation properties
    public ICollection<Bot> Bots { get; set; } = new List<Bot>();
}
