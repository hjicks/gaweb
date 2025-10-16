public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    // Foreign key relationships
    public Channel? Channel { get; set; }

    // Navigation properties
    public ICollection<Bot> Bots { get; set; } = new List<Bot>();
}