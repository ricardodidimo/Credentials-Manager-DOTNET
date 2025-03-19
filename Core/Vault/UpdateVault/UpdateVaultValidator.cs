using FluentValidation;

namespace Core.Vault.UpdateVault
{
    public class UpdateVaultValidator : AbstractValidator<UpdateVaultDTO>
    {
        public UpdateVaultValidator()
        {
            RuleFor(entity => entity.VaultId).NotEmpty();
            RuleFor(entity => entity.Name).NotEmpty().MaximumLength(50).When(x => x.Name is not null);
            RuleFor(entity => entity.Description).NotEmpty().MaximumLength(255).When(x => x.Description is not null);
            RuleFor(entity => entity.AccessCode).NotEmpty().MaximumLength(255).When(x => x.AccessCode is not null); ;
        }
    }
}
