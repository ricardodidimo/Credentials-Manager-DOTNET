using Core.Category.CreateCategory;
using Core.User;
using Core.Vault;
using FluentResults;
using webapi.Helpers;
using Core.Category.ListCategories;
using Infrastructure.Repositories.EfCore;
using Microsoft.AspNetCore.Mvc;
using Core.Category;
using webapi.Endpoints.Vault;
using Core.Credentials.ListCredentials;
using Core.Credentials;
using Core;

namespace webapi.Endpoints.Credentials
{
    public class ListCredentialsEndpoint
    {
        /// <summary>
        /// List credentials contained by a certain vault
        /// </summary>
        /// 
        /// <param name="vaultId">The unique identifier of the targeted vault</param>
        /// <param name="accessCode">The access code registered for the targeted vault container</param>
        /// <param name="categoryId">Optional filter to reduce listing by specific category under a vault container</param>
        /// <param name="page">Numeric identifier of the targeted page for listing</param>
        /// <param name="pageSize">Optional parameter that defines a page's length</param>
        ///         
        /// <response code="200">Returns a collection of credentials records</response>
        /// <response code="400">General fail on processing request</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// Returns a list of credentials
        /// </returns>
        public static async Task<IResult> Execute([FromQuery] string vaultId, [FromQuery] string accessCode, 
            [FromQuery] string? categoryId, [FromQuery] int page, [FromQuery] int pageSize,
            IAuthorizationManagerJWT accessManager, 
            ICredentialsRepository credentialsRepository, 
            IVaultRepository vaultRepository, 
            ISymmetricEncryptionManager encryptionManager)
        {
            var listUseCase = new ListCredentials(credentialsRepository, vaultRepository, accessManager, encryptionManager);
            var listFilters = new ListCredentialsFiltersDTO(vaultId, accessCode, categoryId, page, pageSize);
            IResult<List<CredentialsModel>> action = await listUseCase.Execute(listFilters);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            List<CredentialsResponseDTO> response = action.Value.Select(x => CredentialsResponseDTO.ToCredentialsResponseDTO(x)).ToList();
            return response.ToApiSuccess();
        }
    }
}
