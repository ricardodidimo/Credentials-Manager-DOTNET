using Core.Vault.DeleteVault;
using Core.Vault;
using Core;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using webapi.Helpers;

namespace webapi.Endpoints.Vault
{
    public class DeleteVaultEndpoint
    {
        /// <summary>
        /// Deletes a specific vault container.
        /// </summary>
        /// 
        /// <remarks>
        /// Upon successful deletion will also remove credentials and categories attached to vault
        /// </remarks>
        /// 
        /// <param name="id">The unique identifier of the vault</param>
        /// <param name="accessCode">The access code registered on vault's creation</param>
        /// 
        /// <response code="204">Returns no data yet indicates a successful delete</response>
        /// <response code="400">If the vault is not found Or access code is incorrect</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// There is no return for this action
        /// </returns>
        public static async Task<IResult> Execute([FromQuery] string id, [FromQuery] string accessCode, IAuthorizationManager accessManager, IVaultRepository vaultRepository)
        {
            var deleteUseCase = new DeleteVault(accessManager, vaultRepository);
            IResult<VaultModel> action = await deleteUseCase.Execute(new DeleteVaultDTO(id, accessCode));
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return action.Value.ToApiNoContent();
        }
    }
}
