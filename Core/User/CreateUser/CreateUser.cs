using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Core.User.CreateUser
{
    public class CreateUser(IUserRepository userRepository)
    {
        public async Task<IResult<User>> Execute(CreateUserDTO userDTO)
        {
            var validationRules = new CreateUserValidator();
            ValidationResult results = validationRules.Validate(userDTO);
            if (results.IsValid is false)
            {
                return Result.Fail<User>(Common.ToResultErrorList(results.Errors));
            }

            string hashPassword = new PasswordHasher<CreateUserDTO>().HashPassword(userDTO, userDTO.PlainPassword);
            string UUID = Guid.NewGuid().ToString();
            var user = new User(UUID, userDTO.Name, userDTO.Email, hashPassword);
            IResult<User> created = await userRepository.CreateUser(user);
            return created;
        }
    }
}
