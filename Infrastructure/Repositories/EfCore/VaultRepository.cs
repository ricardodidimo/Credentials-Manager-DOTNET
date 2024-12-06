using Core.Vault;
using Core.Vault.ListVaults;
using FluentResults;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EfCore
{
    public class VaultRepository(ApplicationDbContext _context) : IVaultRepository
    {
        public const string VAULT_NOT_FOUND = "Unable to find vault";

        public async Task<IResult<List<Vault>>> ListVaults(ListVaultFiltersDTO listingConfig)
        {
            IQueryable<Vault> query = ConstructFilteredQuery(listingConfig);
            var result = await query.Skip((listingConfig.CurrentPage - 1) * listingConfig.PageSize).Take(listingConfig.PageSize).ToListAsync();
            return Result.Ok(result);
        }

        private IQueryable<Vault> ConstructFilteredQuery(ListVaultFiltersDTO config)
        {
            IQueryable<Vault> query = _context.Vaults;
            if (!string.IsNullOrEmpty(config.UserID)) query = query.Where(v => v.UserId .Equals(config.UserID));

            return query;
        }

        public async Task<IResult<Vault>> FindByIdentifier(string identifier)
        {
           var result = await _context.Vaults.Where(u => u.ID.Equals(identifier)).FirstOrDefaultAsync();
           if (result is null)
            {   
                return Result.Fail<Vault>(VAULT_NOT_FOUND);
            }

           return Result.Ok(result);
        }

        public async Task<IResult<Vault>> CreateVault(Vault vault)
        {
            await _context.Vaults.AddAsync(vault);
            var inserts =  await _context.SaveChangesAsync();
            if (inserts is not 1)
            {
                return Result.Fail<Vault>(ErrorMessages.UNABLE_CREATE);   
            }

            return Result.Ok(vault);
        }


        public async Task<IResult<Vault>> UpdateVault(Vault vault)
        {
            _context.Vaults.Update(vault);
            var updates = await _context.SaveChangesAsync();
            if (updates is not 1)
            {
                return Result.Fail<Vault>(ErrorMessages.UNABLE_UPDATE);
            }

            return Result.Ok(vault);
        }

        public async Task<IResult<Vault>> DeleteVault(Vault vault)
        {
            _context.Vaults.Remove(vault);
            var deletes = await _context.SaveChangesAsync();
            if (deletes is not 1)
            {
                return Result.Fail<Vault>(ErrorMessages.UNABLE_DELETE);
            }

            return Result.Ok(vault);
        }
    }
}
