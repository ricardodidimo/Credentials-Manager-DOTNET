using Core.User;
using Core.Vault;
using FluentResults;
using webapi.Helpers;
using Core.Vault.CreateVault;

namespace webapi.Endpoints.Vault
{
    public class CreateVaultEndpoint
    {
        /// <summary>
        /// Creates a new vault container
        /// </summary>
        /// 
        /// <response code="201">Returns info about the created vault</response>
        /// <response code="400">Data validation or resource authorization failed for creation action</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// Returns info about the created vault
        /// </returns>
        /// 
        /// <example>
        /// {
        ///     "Name": "My new vault!",
        ///     "Description": "This vaults will hold my login information for a variety of websites!",
        ///     "AccessCode":"my_secret_vault_access_code_123",
        ///     "UserID": "6032e9d6-5eba-42e7-3d4d-0f15e45eec4a"
        /// }
        /// </example>
        public static async Task<IResult> Execute(CreateVaultDTO createVaultDTO,
            IAuthorizationManagerJWT accessManager, IUserRepository userRepository, IVaultRepository vaultRepository)
        {
            var createUseCase = new CreateVault(userRepository, vaultRepository, accessManager);
            IResult<VaultModel> action = await createUseCase.Execute(createVaultDTO);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return VaultResponseDTO.ToVaultResponseDTO(action.Value).ToApiSuccess(201);
        }
    }
}
