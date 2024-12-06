using Core.Category.CreateCategory;
using Core.Category.ListCategories;
using Core.Credentials.CreateCredentials;
using Core.Credentials.ListCredentials;
using FluentValidation;

namespace Core.Category.DeleteCategory
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryDTO>
    {
        public DeleteCategoryValidator()
        {
            RuleFor(entity => entity.CategoryId).NotEmpty();
        }
    }
}
