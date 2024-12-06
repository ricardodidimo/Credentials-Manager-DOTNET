using Core.Category.DeleteCategory;
using Core.Credentials;
using Core.User;
using Core.Vault;
using Core.Vault.CreateVault;
using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Credentials.DeleteCredentials
{

    public class DeleteCredentials(IAuthorizationManager authorizationManager, ICredentialsRepository credentialsRepository)
    {
        public async Task<IResult<Credentials>> Execute(DeleteCredentialsDTO deleteDTO)
        {
            var validationRules = new DeleteCredentialsValidator();
            ValidationResult results = validationRules.Validate(deleteDTO);
            if (results.IsValid is false)
            {
                return Result.Fail<Credentials>(Common.ToResultErrorList(results.Errors));
            }

            IResult<Credentials> credentialsExists = await credentialsRepository.FindByIdentifier(deleteDTO.CredentialsId);
            if (credentialsExists.IsFailed) {
                return Result.Fail<Credentials>(Common.CREDENTIALS_REFERENCE_NOT_FOUND_ERR);
            }

            Credentials credentials = credentialsExists.Value;
            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(credentials.Vault!.UserId);
            if (!callerIsResourceOwner) {
                return Result.Fail<Credentials>(Common.LACK_OWNERSHIP_ERR);
            }

            PasswordVerificationResult accessCodeCompare = new PasswordHasher<Credentials>().VerifyHashedPassword(credentials, credentials.Vault.AccessCode, deleteDTO.VaultAccessCode);
            if (accessCodeCompare.Equals(PasswordVerificationResult.Failed))
            {
                return Result.Fail<Credentials>(Common.VAULT_ACCESS_DENIED_ERR);
            }

            return await credentialsRepository.DeleteCredentials(credentials);
        }
    }
    }
