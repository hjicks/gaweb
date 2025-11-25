namespace MASsenger.Application.Interfaces
{
    public interface IJwtService
    {
        string GetJwt(Int32 userId, IEnumerable<string> roles);
    }
}
