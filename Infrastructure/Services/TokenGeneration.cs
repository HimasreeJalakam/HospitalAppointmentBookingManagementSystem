using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenGeneration
    {
        private readonly IConfiguration _configuration;
        public TokenGeneration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJWT(string personId, string role, string name, string email)
        {
            var key = _configuration.GetValue<string>("Jwt:Key");

            if (string.IsNullOrEmpty(key) || key.Length < 32)
                throw new Exception("JWT Key must be at least 32 characters long.");

            var securedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var securityCredentials = new SigningCredentials(securedKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, personId),
        new Claim("role", role),
        new Claim("name", name),
        new Claim("email", email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: securityCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
