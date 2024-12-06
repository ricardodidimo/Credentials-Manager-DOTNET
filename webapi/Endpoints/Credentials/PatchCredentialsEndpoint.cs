using Core.Category.CreateCategory;
using Core.User;
using Core.Vault;
using FluentResults;
using webapi.Helpers;
using Core.Category;
using Core;
using Core.Credentials.UpdateCredentials;
using Core.Credentials;

namespace webapi.Endpoints.Credentials
{
    public class PatchCredentialsEndpoint
    {
        /// <summary>
        /// Updates selected info about an existing credentials pair
        /// </summary>
        /// 
        /// <response code="200">Returns info about updated credentials</response>
        /// <response code="400">Data validation or resource authorization failed for update action</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// Returns info about the updated credentials
        /// </returns>
        /// 
        /// <example>
        /// {
        ///      "Name": "My updated credentials!",
        ///      "Description": "my new google login info! "
        ///      "PrimaryCredential": "thatsmygoogleemail@gmail.com"
        ///      "SecondaryCredential": "my_rotated_p@ssw0rd"
        ///      "VaultId": "6032e9d6-5eba-42e7-3d4d-0f15e45eec4a"
        ///      "CategoryId": "9232e9d6-5eba-c2e7-334d-0f39035eec4a"
        /// }
        /// </example>
        public static async Task<IResult> Execute(UpdateCredentialsDTO updateDTO,
            IAuthorizationManagerJWT accessManager,
            ICredentialsRepository credentialsRepository,
            ISymmetricEncryptionManager encryptionManager,
            IVaultRepository vaultRepository,
            ICategoryRepository categoryRepository)
        {
            var updateUseCase = new UpdateCredentials(accessManager, credentialsRepository, encryptionManager, vaultRepository, categoryRepository);
            IResult<CredentialsModel> action = await updateUseCase.Execute(updateDTO);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return CredentialsResponseDTO.ToCredentialsResponseDTO(action.Value).ToApiSuccess();
        }
    }
}
