using Core.Category.CreateCategory;
using Core.Category.ListCategories;
using Core.Credentials.CreateCredentials;
using Core.Credentials.ListCredentials;
using FluentValidation;

namespace Core.Category.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(entity => entity.CategoryId).NotEmpty();
            RuleFor(entity => entity.Name).NotEmpty().MaximumLength(50).When(x => x.Name is not null);
        }
    }
}
