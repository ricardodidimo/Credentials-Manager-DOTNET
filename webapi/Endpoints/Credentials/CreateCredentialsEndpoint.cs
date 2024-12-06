using Core.Category;
using Core.Vault;
using FluentResults;
using webapi.Helpers;
using Core.Credentials.CreateCredentials;
using Core.Credentials;
using Core;

namespace webapi.Endpoints.Credentials
{
    public class CreateCredentialsEndpoint
    {
        /// <summary>
        /// Creates new credentials under specific vault
        /// </summary>
        /// 
        /// <response code="201">Returns info about the created credentials</response>
        /// <response code="400">Data validation or resource authorization failed for creation action</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// Returns info about the created credentials
        /// </returns>
        /// 
        /// <example>
        /// {
        ///      "Name": "My new credentials pair!",
        ///      "Description": "my google login info! "
        ///      "PrimaryCredential": "thatsmygoogleemail@gmail.com"
        ///      "SecondaryCredential": "my_secret_p@ssw0rd"
        ///      "VaultId": "6032e9d6-5eba-42e7-3d4d-0f15e45eec4a"
        ///      "CategoryId": "9232e9d6-5eba-c2e7-334d-0f39035eec4a"
        /// }
        /// </example>
        public static async Task<IResult> Execute(CreateCredentialsDTO createCredentialsDTO,
            IAuthorizationManagerJWT accessManager,
            IVaultRepository vaultRepository,
            ICredentialsRepository credentialsRepository,
            ICategoryRepository categoryRepository,
            ISymmetricEncryptionManager encryptionManager)
        {
            var createUseCase = new CreateCredentials(vaultRepository, credentialsRepository, categoryRepository, accessManager, encryptionManager);
            IResult<CredentialsModel> action = await createUseCase.Execute(createCredentialsDTO);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return CredentialsResponseDTO.ToCredentialsResponseDTO(action.Value).ToApiSuccess(201);
        }
    }
}
