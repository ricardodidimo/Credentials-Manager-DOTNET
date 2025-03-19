using FluentResults;
using webapi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Core;
using Core.Credentials.DeleteCredentials;
using Core.Credentials;

namespace webapi.Endpoints.Credentials
{
    public class DeleteCredentialsEndpoint
    {
        /// <summary>
        /// Deletes specific credentials
        /// </summary>
        /// 
        /// <param name="id">The unique identifier of the credentials</param>
        /// <param name="accessCode">The access code registered for the targeted vault container</param>
        /// 
        /// <response code="204">Returns no data yet indicates a successful delete</response>
        /// <response code="400">If the credentials pair is not found Or resource authorization failed for delete action</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// There is no return for this action
        /// </returns>
        public static async Task<IResult> Execute(string id, [FromQuery] string accessCode,
            IAuthorizationManager accessManager, ICredentialsRepository repository)
        {
            var deleteUseCase = new DeleteCredentials(accessManager, repository);
            IResult<CredentialsModel> action = await deleteUseCase.Execute(new DeleteCredentialsDTO(id, accessCode));
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return action.Value.ToApiNoContent();
        }
    }
}
