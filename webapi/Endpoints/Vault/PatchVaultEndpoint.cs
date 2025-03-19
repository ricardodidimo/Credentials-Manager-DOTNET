using Core.Vault;
using FluentResults;
using webapi.Helpers;
using Core.Vault.UpdateVault;
using Core;

namespace webapi.Endpoints.Vault
{
    public class PatchVaultEndpoint
    {
        /// <summary>
        /// Updates selected info about an existing vault
        /// </summary>
        /// 
        /// <response code="200">Returns info about the updated vault</response>
        /// <response code="400">Data validation or resource authorization failed for update action</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// Returns info about the updated vault
        /// </returns>
        /// 
        /// <example>
        /// {
        ///     "VaultId": "6032e9d6-5eba-42e7-3d4d-0f15e45eec4a"
        ///      "Name": "My updated vault!",
        ///      "Description": "This vaults will hold my login information for a variety of websites!",
        ///      "AccessCode":"my_updated_secret_vault_access_code_123"
        /// }
        /// </example>
        public static async Task<IResult> Execute(UpdateVaultDTO updateVaultDTO, IAuthorizationManager accessManager, IVaultRepository vaultRepository)
        {
            var updateUseCase = new UpdateVault(accessManager, vaultRepository);
            IResult<VaultModel> action = await updateUseCase.Execute(updateVaultDTO);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return VaultResponseDTO.ToVaultResponseDTO(action.Value).ToApiSuccess();
        }
    }
}
