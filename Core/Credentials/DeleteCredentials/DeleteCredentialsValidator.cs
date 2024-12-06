using FluentValidation;

namespace Core.Credentials.DeleteCredentials
{
    public class DeleteCredentialsValidator : AbstractValidator<DeleteCredentialsDTO>
    {
        public DeleteCredentialsValidator()
        {
            RuleFor(entity => entity.CredentialsId).NotEmpty();
            RuleFor(entity => entity.VaultAccessCode).NotEmpty();
        }
    }
}
