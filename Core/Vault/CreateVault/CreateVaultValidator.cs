using FluentValidation;

namespace Core.Vault.CreateVault
{
    public class CreateVaultValidator : AbstractValidator<CreateVaultDTO>
    {
        public CreateVaultValidator()
        {
            RuleFor(vault => vault.Name).NotEmpty().MaximumLength(75);
            RuleFor(vault => vault.Description).NotEmpty().MaximumLength(255);
            RuleFor(vault => vault.AccessCode).NotEmpty().MaximumLength(255);
            RuleFor(vault => vault.UserID).NotEmpty();
        }
    }
}
