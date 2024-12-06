using Core.Credentials.CreateCredentials;
using FluentValidation;

namespace Core.Credentials.CreateCredentials
{
    public class CreateCredentialsValidator : AbstractValidator<CreateCredentialsDTO>
    {
        public CreateCredentialsValidator()
        {
            RuleFor(vault => vault.Name).NotEmpty().MaximumLength(75);
            RuleFor(vault => vault.Description).NotEmpty().MaximumLength(255);
            RuleFor(vault => vault.PrimaryCredential).MaximumLength(2055);
            RuleFor(vault => vault.SecondaryCredential).MaximumLength(2055);
            RuleFor(vault => vault.VaultId).NotEmpty();
        }
    }
}
