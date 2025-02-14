using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Adventour.Api.Services.Authentication
{
    public sealed class JwtTokenProviderService : ITokenProviderService
    {

        private readonly string secretKey;
        private readonly SymmetricSecurityKey securityKey;
        private readonly SigningCredentials credential;
        public JwtTokenProviderService()
        {
            this.secretKey = Environment.GetEnvironmentVariable("JWT_SECRET")!;
            this.securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            this.credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        }

        public string Create(string userId)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                   new Claim(JwtRegisteredClaimNames.Sub, string.IsNullOrEmpty(userId) ? "anonymous" : "user"),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                   new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credential,
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            };

            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public string GeneratePinToken(string email, string confirmationPin)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                   new Claim("email", email),
                   new Claim("pin", confirmationPin),
                   new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                   new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credential,
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            };

            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public async Task<bool >ValidatePinToken(string token, string enteredPin, string email)
        {
            var tokenHandler = new JsonWebTokenHandler();

            try
            {
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuerSigningKey = true
                };

                var validationResult = await tokenHandler.ValidateTokenAsync(token, parameters);
                
                validationResult.Claims.TryGetValue("email", out var tokenEmail);
                validationResult.Claims.TryGetValue("pin", out var pin);

                return pin?.ToString()?.Equals(enteredPin) ?? false; // PIN matches
            }
            catch
            {
                return false;
            }
        }
    }
}
