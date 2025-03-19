using Core.Category;
using Core.Vault;
using FluentResults;
using FluentValidation.Results;

namespace Core.Credentials.CreateCredentials
{
    public class CreateCredentials(
        IVaultRepository vaultRepository, 
        ICredentialsRepository credentialsRepository, 
        ICategoryRepository categoryRepository, 
        IAuthorizationManager authorizationManager,
        ISymmetricEncryptionManager encryptionManager
    ) {
        public async Task<IResult<Credentials>> Execute(CreateCredentialsDTO credentialsDTO)
        {
            var validationRules = new CreateCredentialsValidator();
            ValidationResult results = validationRules.Validate(credentialsDTO);
            if (results.IsValid is false)
            {
                return Result.Fail<Credentials>(Common.ToResultErrorList(results.Errors));
            }

            IResult<Vault.Vault> vaultExists = await vaultRepository.FindByIdentifier(credentialsDTO.VaultId);
            if (vaultExists.IsFailed)
            {
                return Result.Fail<Credentials>(vaultExists.Errors);
            }

            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(vaultExists.Value.UserId);
            if (!callerIsResourceOwner)
            {
                return Result.Fail<Credentials>(Common.LACK_OWNERSHIP_ERR);
            }

            IResult<Category.Category>? category = null;
            if (credentialsDTO.CategoryId is not null)
            {
                category = await categoryRepository.FindByIdentifier(credentialsDTO.CategoryId);
                callerIsResourceOwner = authorizationManager.VerifyOwnership(category.Value.UserId);
                if (!callerIsResourceOwner)
                {
                    return Result.Fail<Credentials>(Common.LACK_OWNERSHIP_ERR);
                }
            }

            string UUID = Guid.NewGuid().ToString();
            byte[] encryptedPrimaryCredential = encryptionManager.Encrypt(credentialsDTO.PrimaryCredential);
            byte[] encryptedSecondaryCredential = encryptionManager.Encrypt(credentialsDTO.SecondaryCredential);

            var credentials = new Credentials
            {
                ID = UUID,
                Name = credentialsDTO.Name,
                Description = credentialsDTO.Description,
                Vault = vaultExists.Value,
                VaultId = vaultExists.Value.ID,
                Category = category?.Value,
                CategoryId = category?.Value?.ID,
                PrimaryCredential = Convert.ToBase64String(encryptedPrimaryCredential),
                SecondaryCredential = Convert.ToBase64String(encryptedSecondaryCredential),
            };

            return await credentialsRepository.CreateCredentials(credentials);
        }
    }
}
