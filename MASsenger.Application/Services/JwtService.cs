using MASsenger.Application.Interfaces;
using MASsenger.Core.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MASsenger.Application.Services
{
    internal class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        public JwtService(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public string GetJwt(Int32 baseUserId, IEnumerable<string> roles)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, baseUserId.ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_jwtOptions.ExpiryInMins)),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
