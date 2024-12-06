using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Core.Vault.DeleteVault
{

    public class DeleteVault(IAuthorizationManager authorizationManager, IVaultRepository vaultRepository)
    {
        public async Task<IResult<Vault>> Execute(DeleteVaultDTO deleteDTO)
        {
            IResult<Vault> vaultExists = await vaultRepository.FindByIdentifier(deleteDTO.VaultId);
            if (vaultExists.IsFailed) {
                return Result.Fail<Vault>(Common.VAULT_REFERENCE_NOT_FOUND_ERR);
            }

            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(vaultExists.Value.UserId);
            if (!callerIsResourceOwner) {
                return Result.Fail<Vault>(Common.LACK_OWNERSHIP_ERR);
            }

            Vault vault = vaultExists.Value;
            PasswordVerificationResult passResult = new PasswordHasher<Vault>().VerifyHashedPassword(vault, vault.AccessCode, deleteDTO.VaultAccessCode);
            if (passResult.Equals(PasswordVerificationResult.Failed))
            {
                return Result.Fail<Vault>(Common.VAULT_ACCESS_DENIED_ERR);
            }

            return await vaultRepository.DeleteVault(vaultExists.Value);
        }
    }
    }
