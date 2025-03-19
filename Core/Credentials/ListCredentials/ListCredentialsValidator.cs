using Core.Credentials.ListCredentials;
using FluentValidation;

namespace Core.Credentials.CreateCredentials
{
    public class ListCredentialsValidator : AbstractValidator<ListCredentialsFiltersDTO>
    {
        public ListCredentialsValidator()
        {
            RuleFor(list => list.VaultId).NotEmpty();
            RuleFor(list => list.VaultAccessCode).NotEmpty();
            RuleFor(list => list.PageSize).GreaterThanOrEqualTo(1);
            RuleFor(list => list.CurrentPage).GreaterThanOrEqualTo(1);
        }
    }
}
