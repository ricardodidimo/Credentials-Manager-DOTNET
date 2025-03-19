using Core.User;
using FluentResults;
using FluentValidation.Results;

namespace Core.Vault.ListVaults
{
    public class ListVaults(IUserRepository userRepository, IVaultRepository vaultRepository, IAuthorizationManager authorizationManager)
    {
        public async Task<IResult<List<Vault>>> Execute(ListVaultFiltersDTO listingConfig)
        {
            var validationRules = new ListVaultValidator();
            ValidationResult results = validationRules.Validate(listingConfig);
            if (results.IsValid is false)
            {
                return Result.Fail<List<Vault>>(Common.ToResultErrorList(results.Errors));
            }

            IResult<User.User> vaultOwnerExists = await userRepository.FindByIdentifier(listingConfig.UserID);
            if (vaultOwnerExists.IsFailed)
            {
                return Result.Fail<List<Vault>>(vaultOwnerExists.Errors);
            }

            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(vaultOwnerExists.Value.ID);
            if (!callerIsResourceOwner)
            {
                return Result.Fail<List<Vault>>(Common.LACK_OWNERSHIP_ERR);
            }

            var listing = await vaultRepository.ListVaults(listingConfig);
            if (listing.IsFailed)
            {
                return Result.Fail<List<Vault>>(listing.Errors);
            }

            return Result.Ok(listing.Value);
        }
    }
}
