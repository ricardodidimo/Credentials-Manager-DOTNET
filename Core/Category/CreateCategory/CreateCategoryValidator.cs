using Core.Category.CreateCategory;
using FluentValidation;

namespace Core.Credentials.CreateCredentials
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryValidator()
        {
            RuleFor(entity => entity.Name).NotEmpty().MaximumLength(50);
            RuleFor(entity => entity.UserId).NotEmpty();
        }
    }
}
