using MAS.Application.Interfaces;
using MAS.Core.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace MAS.Application.Services;

public record PasswordHash(byte[] Hash, byte[] Salt);
public record TokenHash(byte[] Hash, string RefreshToken);

public sealed class HashService : IHashService
{
    private const int passwordSaltSize = 16;
    private const int passwordHashSize = 32;
    private const int refreshTokenSizeBeforeEncode = 32;
    private const int pbkdf2Iterations = 100_000;
    private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
    private readonly TokenOptions _tokenOptions;

    public HashService(IOptions<TokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public PasswordHash HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(passwordSaltSize);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password, salt,
            pbkdf2Iterations, hashAlgorithm);

        var hash = pbkdf2.GetBytes(passwordHashSize);

        return new PasswordHash(hash, salt);
    }

    public bool VerifyPassword(string password, PasswordHash stored)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password, stored.Salt,
            pbkdf2Iterations, hashAlgorithm);

        var computed = pbkdf2.GetBytes(stored.Hash.Length);

        return CryptographicOperations.FixedTimeEquals(
            computed, stored.Hash);
    }

    public TokenHash CreateAndHashRefreshToken()
    {
        var refreshToken = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(refreshTokenSizeBeforeEncode));

        using var hmac = new HMACSHA256(
            Encoding.UTF8.GetBytes(_tokenOptions.RefreshToken.Key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));

        return new TokenHash(hash, refreshToken);
    }

    public byte[] HashRefreshToken(string refreshToken)
    {
        using var hmac = new HMACSHA256(
            Encoding.UTF8.GetBytes(_tokenOptions.RefreshToken.Key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));

        return hash;
    }
}
