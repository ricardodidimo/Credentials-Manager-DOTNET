using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.User.AuthenticateUser
{
    public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserDTO>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(login => login.Email).NotEmpty();
            RuleFor(login => login.Password).NotEmpty();
        }
    }
}
