using Core.Category;
using Core.Vault;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Core.Credentials.UpdateCredentials
{

    public class UpdateCredentials(
        IAuthorizationManager authorizationManager, 
        ICredentialsRepository credentialsRepository,
        ISymmetricEncryptionManager encryptionManager,
        IVaultRepository vaultRepository,
        ICategoryRepository categoryRepository)
    {
        public async Task<IResult<Credentials>> Execute(UpdateCredentialsDTO updateDTO)
        {
            IResult<Credentials> credentialsExists = await credentialsRepository.FindByIdentifier(updateDTO.CredentialsId);
            if (credentialsExists.IsFailed) {
                return Result.Fail<Credentials>(Common.CREDENTIALS_REFERENCE_NOT_FOUND_ERR);
            }

            Credentials credentials = credentialsExists.Value;
            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(credentials.Vault!.UserId);
            if (!callerIsResourceOwner) {
                return Result.Fail<Credentials>(Common.LACK_OWNERSHIP_ERR);
            }

            PasswordVerificationResult accessCodeCompare = new PasswordHasher<Credentials>().VerifyHashedPassword(credentials, credentials.Vault.AccessCode, updateDTO.VaultAccessCode);
            if (accessCodeCompare.Equals(PasswordVerificationResult.Failed))
            {
                return Result.Fail<Credentials>(Common.VAULT_ACCESS_DENIED_ERR);
            }

            if (updateDTO.Name is not null) credentials.Name = updateDTO.Name;
            if (updateDTO.Description is not null) credentials.Description = updateDTO.Description;

            if (updateDTO.PrimaryCredential is not null)
            {
                byte[] encryptedPrimaryCredential = encryptionManager.Encrypt(updateDTO.PrimaryCredential);
                credentials.PrimaryCredential = Convert.ToBase64String(encryptedPrimaryCredential);
            }

            if (updateDTO.SecondaryCredential is not null)
            {
                byte[] encryptedSecondaryCredential = encryptionManager.Encrypt(updateDTO.SecondaryCredential);
                credentials.SecondaryCredential = Convert.ToBase64String(encryptedSecondaryCredential);
            }

            if (updateDTO.VaultId is not null) {
                IResult<Vault.Vault> vaultExists = await vaultRepository.FindByIdentifier(updateDTO.VaultId);
                if (vaultExists.IsFailed)
                {
                    return Result.Fail<Credentials>(Common.VAULT_REFERENCE_NOT_FOUND_ERR);
                }

                callerIsResourceOwner = authorizationManager.VerifyOwnership(vaultExists.Value.UserId);
                if (!callerIsResourceOwner)
                {
                    return Result.Fail<Credentials>(Common.LACK_OWNERSHIP_ERR);
                }

                credentials.Vault = vaultExists.Value;
                credentials.VaultId = vaultExists.Value.ID;
            }

            if (updateDTO.CategoryId is not null)
            {
                IResult<Category.Category> categoryExists = await categoryRepository.FindByIdentifier(updateDTO.CategoryId);
                if (categoryExists.IsFailed)
                {
                    return Result.Fail<Credentials>(Common.CATEGORY_REFERENCE_NOT_FOUND_ERR);
                }

                callerIsResourceOwner = authorizationManager.VerifyOwnership(categoryExists.Value.UserId);
                if (!callerIsResourceOwner)
                {
                    return Result.Fail<Credentials>(Common.LACK_OWNERSHIP_ERR);
                }

                credentials.Category = categoryExists.Value;
                credentials.CategoryId = categoryExists.Value.ID;
            }

            return await credentialsRepository.UpdateCredentials(credentials);
        }
    }
    }
