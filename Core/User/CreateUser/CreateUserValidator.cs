using FluentValidation;

namespace Core.User.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().MinimumLength(3).MaximumLength(255);
            RuleFor(user => user.Email).NotEmpty().MaximumLength(254);
            RuleFor(user => user.PlainPassword).NotEmpty();
        }
    }
}
