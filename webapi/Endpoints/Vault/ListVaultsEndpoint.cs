using Core.User;
using Core.Vault;
using FluentResults;
using webapi.Helpers;
using Core.Vault.ListVaults;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Endpoints.Vault
{
    public class ListVaultsEndpoint
    {
        /// <summary>
        /// List vaults for targeted user
        /// </summary>
        /// 
        /// <param name="userId">The unique identifier of the targeted user</param>
        /// <param name="page">Numeric identifier of the targeted page for listing</param>
        /// <param name="pageSize">Optional parameter that defines a page's length</param>
        ///
        /// <response code="200">Returns a collection of vaults</response>
        /// <response code="400">General fail on processing request</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// Returns a list of vaults
        /// </returns>
        public static async Task<IResult> Execute([FromQuery] string userId,
            [FromQuery] int page, [FromQuery] int? pageSize,
            IAuthorizationManagerJWT accessManager, IUserRepository userRepository, IVaultRepository vaultRepository)
        {
            var listUseCase = new ListVaults(userRepository, vaultRepository, accessManager);
            var listFilters = new ListVaultFiltersDTO(userId, page, pageSize ?? 1);
            IResult<List<VaultModel>> action = await listUseCase.Execute(listFilters);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            List<VaultResponseDTO> response = action.Value.Select(x => VaultResponseDTO.ToVaultResponseDTO(x)).ToList();
            return response.ToApiSuccess();
        }
    }
}
