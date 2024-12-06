using Core.User;
using webapi.Helpers;
using Core.User.AuthenticateUser;
using FluentResults;

namespace webapi.Endpoints.User
{
    public class LoginUserEndpoint
    {
        /// <summary>
        /// Authenticate as an user
        /// </summary>
        /// 
        /// <response code="201">Returns info about the authenticated user</response>
        /// <response code="400">Data validation failed for authentication action</response>
        /// 
        /// <returns>
        /// Returns info about the authenticated user
        /// </returns>
        /// 
        /// <example>
        /// {
        ///      "Email": "mypersonalemail@domain.com"
        ///      "Password": "safe_lengthy_password!"
        /// }
        /// </example>
        public static async Task<IResult> Execute(AuthenticateUserDTO req, JWTokenGenerator JWTManager, IUserRepository userRepo)
        {
            var authUseCase = new AuthenticateUser(userRepo);
            IResult<UserModel> action = await authUseCase.Execute(req);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError();
            }

            UserModel user = action.Value;
            IResult<string> tokenGeneration = JWTManager.Generate(user.ID, user.Email);
            if (tokenGeneration.IsFailed)
            {
                return action.Errors.ToApiError();
            }

            return UserTokenResponseDTO.ToUserTokenResponseDTO(user, tokenGeneration.Value).ToApiSuccess();
        }
    }
}
