public class BaseUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Username { get; set; } = null;
    public string? Description { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsVerified { get; set; } = false;
    
}
