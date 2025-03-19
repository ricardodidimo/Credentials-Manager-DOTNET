using Core;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace webapi.Helpers
{
    public class AuthorizationManagerJWT(IHttpContextAccessor _httpContextAccessor) : IAuthorizationManagerJWT
    {   
        public bool VerifyOwnership(string identifier)
        {
            return GetCurrentUserIdentifierFromToken().Equals(identifier);
        }

        public string GetCurrentUserIdentifierFromToken()
        {
            string userIdReferenceClaimKey = JWTokenGenerator.USER_ID_CLAIM_KEY;
            Dictionary<string, string> claims = GetCurrentUserClaims();
            if (claims.ContainsKey(userIdReferenceClaimKey) is false)
            {
                throw new Exception("missing JWT claim");
            }

            return claims[userIdReferenceClaimKey];
        }

        public Dictionary<string, string> GetCurrentUserClaims()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is null)
            {
                throw new Exception("unauthenticated context");
            }

            return user.Claims.ToDictionary(c => c.Type, c => c.Value);
        }
    }
}
