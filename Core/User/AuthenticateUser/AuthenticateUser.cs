using Core.User.CreateUser;
using FluentResults;
using FluentValidation.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.User.AuthenticateUser
{
    public class AuthenticateUser(IUserRepository userRepository)
    {
        private readonly string INCORRECT_CREDENTIALS = "INCORRECT CREDENTIALS";

        public async Task<IResult<User>> Execute(AuthenticateUserDTO authDTO)
        {
            //  validate input
            var validationRules = new AuthenticateUserValidator();
            ValidationResult results = validationRules.Validate(authDTO);
            if (results.IsValid is false)
            {
                return Result.Fail<User>(Common.ToResultErrorList(results.Errors));
            }

            // find by email
            var search = await userRepository.FindByEmail(authDTO.Email);
            if (search.IsFailed)
            {
                return Result.Fail<User>(INCORRECT_CREDENTIALS);
            }

            // compare password
            User user = search.Value;
            PasswordVerificationResult passResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, authDTO.Password);
            if (passResult.Equals(PasswordVerificationResult.Failed))
            {
                return Result.Fail<User>(INCORRECT_CREDENTIALS);
            }

            //// generate jwt token
            //var token = _JWTokenGenerator.Generate(user);
            return Result.Ok<User>(user);
        }
    }
}
