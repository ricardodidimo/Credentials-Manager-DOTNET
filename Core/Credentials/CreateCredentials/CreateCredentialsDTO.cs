namespace Core.Credentials.CreateCredentials
{
    public record CreateCredentialsDTO(string Name, string Description, string PrimaryCredential, string SecondaryCredential, string VaultId, string? CategoryId);
}
