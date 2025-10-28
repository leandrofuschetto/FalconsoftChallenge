using Microsoft.IdentityModel.Tokens;
using RecruitingChallenge.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecruitingChallenge.API.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration config)
        {
            _configuration = config;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyToken = _configuration.GetSection("ConfigForJwt").Value;
            var key = Encoding.ASCII.GetBytes(keyToken);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenResponse = tokenHandler.WriteToken(token);

            if (token != null)
                return tokenResponse;

            throw new Exception("//to do custom - Error generating JWT token");
        }
    }
}