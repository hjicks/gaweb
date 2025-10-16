namespace MASsenger
{
    public class Bot : BaseUser
    {
        public string Token { get; set; } = null!;
        public bool IsActive { get; set; } = false;

        // Navigation properties
        public ICollection<User> Members { get; set; } = new List<User>();
    }
}