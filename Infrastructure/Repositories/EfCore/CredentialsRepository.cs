using Core.Credentials;
using Core.Credentials.ListCredentials;
using FluentResults;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EfCore
{
    public class CredentialsRepository(ApplicationDbContext _context) : ICredentialsRepository
    {
        public const string CREDENTIALS_NOT_FOUND = "Unable to find credentials";

        public async Task<IResult<List<Credentials>>> ListCredentials(ListCredentialsFiltersDTO config)
        {
            IQueryable<Credentials> query = ConstructFilteredQuery(config);
            var result = await query.Skip((config.CurrentPage - 1) * config.PageSize).Take(config.PageSize).ToListAsync();
            return Result.Ok(result);
        }

        private IQueryable<Credentials> ConstructFilteredQuery(ListCredentialsFiltersDTO config)
        {
            IQueryable<Credentials> query = _context.Credentials;
            if (!string.IsNullOrEmpty(config.VaultId)) query = query.Where(c => c.VaultId.Equals(config.VaultId));
            if (!string.IsNullOrEmpty(config.CategoryId)) query = query.Where(c => c.CategoryId!.Equals(config.CategoryId));

            return query;
        }

        public async Task<IResult<Credentials>> FindByIdentifier(string uuid)
        {
            var result = await _context.Credentials.Where(c => c.ID.Equals(uuid)).Include(c => c.Vault).Include(c => c.Category).FirstOrDefaultAsync();
            if (result is null)
            {
                return Result.Fail<Credentials>(CREDENTIALS_NOT_FOUND);
            }

            return Result.Ok(result);
        }

        public async Task<IResult<Credentials>> CreateCredentials(Credentials credentials)
        {
            await _context.Credentials.AddAsync(credentials);
            var inserts = await _context.SaveChangesAsync();
            if (inserts is not 1)
            {
                return Result.Fail<Credentials>(ErrorMessages.UNABLE_CREATE);
            }

            return Result.Ok(credentials);
        }


        public async Task<IResult<Credentials>> UpdateCredentials(Credentials credentials)
        {
            _context.Credentials.Update(credentials);
            var updates = await _context.SaveChangesAsync();
            if (updates is not 1)
            {
                return Result.Fail<Credentials>(ErrorMessages.UNABLE_UPDATE);
            }

            return Result.Ok(credentials);
        }

        public async Task<IResult<Credentials>> DeleteCredentials(Credentials credentials)
        {
            _context.Credentials.Remove(credentials);
            var deletes = await _context.SaveChangesAsync();
            if (deletes is not 1)
            {
                return Result.Fail<Credentials>(ErrorMessages.UNABLE_DELETE);
            }

            return Result.Ok(credentials);
        }
    }
}
