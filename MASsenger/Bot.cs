public class Bot : BaseUser
{
    public string Token { get; set; } = "";
    public bool IsActive { get; set; } = false;

    // Foreign key relationships
    public string Creator { get; set; } // User foreign key

    // Navigation properties
    public ICollection<Group> Groups { get; set; } = new List<Group>();
    public ICollection<Channel> Channels { get; set; } = new List<Channel>();
}
