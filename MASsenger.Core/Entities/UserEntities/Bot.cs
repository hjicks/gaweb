namespace MASsenger.Core.Entities.UserEntities
{
    public class Bot : BaseUser
    {
        public string Token { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;

        // required one-to-many relationship with user
        public User Owner { get; set; } = null!;

        // many-to-many relationships
        public ICollection<User> Members { get; set; } = new List<User>();
    }
}
