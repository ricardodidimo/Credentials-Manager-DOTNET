namespace Core.Vault.ListVaults
{
    public record ListVaultFiltersDTO(string UserID, int CurrentPage = 1, int PageSize = 10);
}
