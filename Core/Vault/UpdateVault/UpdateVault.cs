using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Core.Vault.UpdateVault
{
    public class UpdateVault(IAuthorizationManager authorizationManager, IVaultRepository vaultRepository)
    {
        public async Task<IResult<Vault>> Execute(UpdateVaultDTO updateVaultDTO)
        {
            IResult<Vault> vaultExists = await vaultRepository.FindByIdentifier(updateVaultDTO.VaultId);
            if (vaultExists.IsFailed)
            {
                return Result.Fail<Vault>(Common.VAULT_REFERENCE_NOT_FOUND_ERR);
            }

            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(vaultExists.Value.UserId);
            if (!callerIsResourceOwner)
            {
                return Result.Fail<Vault>(Common.LACK_OWNERSHIP_ERR);
            }

            Vault currentVault = vaultExists.Value; 
            if (updateVaultDTO.Name is not null) currentVault.Name = updateVaultDTO.Name;
            if (updateVaultDTO.Description is not null) currentVault.Description = updateVaultDTO.Description;
            if (updateVaultDTO.AccessCode is not null)
            {
                string hashAccessCode = new PasswordHasher<UpdateVaultDTO>().HashPassword(updateVaultDTO, updateVaultDTO.AccessCode);
                currentVault.AccessCode = hashAccessCode;
            }
            return await vaultRepository.UpdateVault(currentVault);
        }
    }
}
