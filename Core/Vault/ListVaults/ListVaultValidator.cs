using FluentValidation;

namespace Core.Vault.ListVaults
{
    public class ListVaultValidator : AbstractValidator<ListVaultFiltersDTO>
    {
        public ListVaultValidator()
        {
            RuleFor(list => list.UserID).NotEmpty();
            RuleFor(list => list.PageSize).GreaterThanOrEqualTo(1);
            RuleFor(list => list.CurrentPage).GreaterThanOrEqualTo(1);
        }
    }
}
