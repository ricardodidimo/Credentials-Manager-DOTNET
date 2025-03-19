namespace Core
{
    public interface IAuthorizationManager
    {
        bool VerifyOwnership(string identifier);
    }
}
