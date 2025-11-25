namespace MASsenger.Application.Interfaces
{
    public interface IJwtService
    {
        string GetJwt(Int32 userId, string role);
    }
}
