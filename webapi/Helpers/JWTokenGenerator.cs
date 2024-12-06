using FluentResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace webapi.Helpers
{
    public class JWTokenGenerator(IConfiguration _configuration)
    {
        public static readonly Int16 REFRESH_INTERVAL_MINS = 60;
        public static readonly string JWT_KEY_VARIABLE = "jwt_key";
        public static readonly string USER_ID_CLAIM_KEY = JwtRegisteredClaimNames.Jti;

        public byte[]? GetJWTEncryptionKey()
        {
            var encryptKey = _configuration[JWT_KEY_VARIABLE];
            if (encryptKey is null)
            {
                return null;
            }

           return Encoding.ASCII.GetBytes(encryptKey);
        }
        public Result<string> Generate(string identifier, string email)
        {
            var encryptKeyBytes = GetJWTEncryptionKey();
            if (encryptKeyBytes is null)
            {
                return Result.Fail("UNABLE TO GENERATE AUTHENTICATION TOKEN");
            }

            var claims = new List<Claim>
            { 
                new(USER_ID_CLAIM_KEY, identifier),
                new(JwtRegisteredClaimNames.Sub, email)
            };

            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(REFRESH_INTERVAL_MINS),
                SigningCredentials =  new SigningCredentials(new SymmetricSecurityKey(encryptKeyBytes), SecurityAlgorithms.HmacSha256),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            if (token == null)
            {
                return Result.Fail("UNABLE TO GENERATE AUTHENTICATION TOKEN");
            }

            return Result.Ok(tokenHandler.WriteToken(token)!);
        }
    
        public void Configure(JwtBearerOptions x)
        {
            var key = GetJWTEncryptionKey();
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            x.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
            };
        }
    }
}
