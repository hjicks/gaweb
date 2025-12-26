using MAS.Application.Interfaces;
using MAS.Core.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MAS.Application.Services;

internal class JwtService : IJwtService
{
    private readonly TokenOptions _tokenOptions;
    public JwtService(IOptions<TokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public string GetJwt(int userId, IEnumerable<string> roles)
    {
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_tokenOptions.AccessToken.Key));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(_tokenOptions.AccessToken.ExpiryInMins),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
