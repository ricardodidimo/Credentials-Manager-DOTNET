using FluentValidation;

namespace Core.Vault.DeleteVault
{
    public class DeleteVaultValidator : AbstractValidator<DeleteVaultDTO>
    {
        public DeleteVaultValidator()
        {
            RuleFor(entity => entity.VaultId).NotEmpty();
            RuleFor(entity => entity.VaultAccessCode).NotEmpty();
        }
    }
}
