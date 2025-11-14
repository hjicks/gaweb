using System.Security.Claims;

namespace MASsenger.Application.Interfaces
{
    public interface IJwtService
    {
        string GetJwt(List<Claim> claims);
    }
}
