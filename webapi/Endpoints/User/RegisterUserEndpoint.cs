using Core.User.CreateUser;
using Core.User;
using webapi.Helpers;

namespace webapi.Endpoints.User
{
    public class RegisterUserEndpoint
    {
        /// <summary>
        /// Creates a new user in the system
        /// </summary>
        /// 
        /// <response code="201">Returns info about the created user</response>
        /// <response code="400">Data validation failed for creation action</response>
        /// 
        /// <returns>
        /// Returns info about the created user
        /// </returns>
        /// 
        /// <example>
        /// {
        ///      "Name": "my name",
        ///      "Email": "mypersonalemail@domain.com"
        ///      "PlainPassword": "safe_lengthy_password!"
        /// }
        /// </example>
        public static async Task<IResult> Execute(CreateUserDTO req, IUserRepository user)
        {
            var createUseCase = new CreateUser(user);
            var action = await createUseCase.Execute(req);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return UserResponseDTO.ToUserResponseDTO(action.Value).ToApiSuccess(201);
        }
    }
}
