using Core.Vault.ListVaults;
using Core.Vault;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Credentials.CreateCredentials;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Core.Credentials.ListCredentials
{
    public class ListCredentials(ICredentialsRepository credentialsRepository,
        IVaultRepository vaultRepository,
        IAuthorizationManager authorizationManager,
        ISymmetricEncryptionManager encryptionManager)
    {
        public async Task<IResult<List<Credentials>>> Execute(ListCredentialsFiltersDTO listingConfig)
        {
            var validationRules = new ListCredentialsValidator();
            ValidationResult results = validationRules.Validate(listingConfig);
            if (results.IsValid is false)
            {
                return Result.Fail<List<Credentials>>(Common.ToResultErrorList(results.Errors));
            }

            var vaultExists = await vaultRepository.FindByIdentifier(listingConfig.VaultId);
            if (vaultExists.IsFailed)
            {
                return Result.Fail<List<Credentials>>(vaultExists.Errors);
            }

            Vault.Vault vault = vaultExists.Value;
            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(vaultExists.Value.UserId);
            if (!callerIsResourceOwner)
            {
                return Result.Fail<List<Credentials>>(Common.LACK_OWNERSHIP_ERR);
            }

            PasswordVerificationResult passResult = new PasswordHasher<Vault.Vault>().VerifyHashedPassword(vault, vault.AccessCode, listingConfig.VaultAccessCode);
            if (passResult.Equals(PasswordVerificationResult.Failed))
            {
                return Result.Fail<List<Credentials>>(Common.VAULT_ACCESS_DENIED_ERR);
            }

            var listing = await credentialsRepository.ListCredentials(listingConfig);
            if (listing.IsFailed)
            {
                return Result.Fail<List<Credentials>>(listing.Errors);
            }

            listing.Value.ForEach((credentials) =>
            {
                byte[] primCredEncrypted = Convert.FromBase64String(credentials.PrimaryCredential);
                byte[] secCredEncrypted = Convert.FromBase64String(credentials.SecondaryCredential);

                credentials.PrimaryCredential = encryptionManager.Decrypt(primCredEncrypted);
                credentials.SecondaryCredential = encryptionManager.Decrypt(secCredEncrypted);
            });

            return Result.Ok(listing.Value);
        }
    }
}
