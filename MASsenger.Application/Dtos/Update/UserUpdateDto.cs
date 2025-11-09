namespace MASsenger.Application.Dtos.Update
{
    public class UserUpdateDto
    {
        public Int32 Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
