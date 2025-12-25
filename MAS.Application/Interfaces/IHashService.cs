using MAS.Application.Services;

namespace MAS.Application.Interfaces;
public interface IHashService
{
    PasswordHash HashPassword(string password);
    bool VerifyPassword(string password, PasswordHash stored);
    TokenHash CreateAndHashRefreshToken();
    byte[] HashRefreshToken(string refreshToken);
}

