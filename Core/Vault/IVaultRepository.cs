using Core.Vault.ListVaults;
using FluentResults;

namespace Core.Vault
{
    public interface IVaultRepository
    {
        public Task<IResult<List<Vault>>> ListVaults(ListVaultFiltersDTO config);
        public Task<IResult<Vault>> FindByIdentifier(string vaultId);
        public Task<IResult<Vault>> CreateVault(Vault vault);
        public Task<IResult<Vault>> UpdateVault(Vault vault);
        public Task<IResult<Vault>> DeleteVault(Vault vault);
    }
}
