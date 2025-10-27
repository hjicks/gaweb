namespace MASsenger.Core.Dto
{
    public class UserUpdateDto
    {
        public UInt64 Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
