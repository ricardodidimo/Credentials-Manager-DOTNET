using Core;

namespace webapi.Helpers
{
    public interface IAuthorizationManagerJWT : IAuthorizationManager
    {
        public Dictionary<string, string> GetCurrentUserClaims();
        public string GetCurrentUserIdentifierFromToken();
    }
}
