using Core.Category.ListCategories;
using Core.Credentials.CreateCredentials;
using Core.Credentials.ListCredentials;
using FluentValidation;

namespace Core.Credentials.CreateCredentials
{
    public class ListCategoriesValidator : AbstractValidator<ListCategoriesFiltersDTO>
    {
        public ListCategoriesValidator()
        {
            RuleFor(list => list.UserId).NotEmpty();
            RuleFor(list => list.PageSize).GreaterThanOrEqualTo(1);
            RuleFor(list => list.CurrentPage).GreaterThanOrEqualTo(1);
        }
    }
}
