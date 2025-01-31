using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Adventour.Api.Infrastructure.Authentication
{
    public sealed class JwtTokenProvider : ITokenProvider
    {

        public JwtTokenProvider()
        {
            
        }
        public string Create(string email)
        {


            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET")!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                   new Claim(JwtRegisteredClaimNames.Email, email)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credential,
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            };

            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
