using FluentValidation;

namespace Core.Credentials.UpdateCredentials
{
    public class UpdateCredentialsValidator : AbstractValidator<UpdateCredentialsDTO>
    {
        public UpdateCredentialsValidator()
        {
            RuleFor(entity => entity.CredentialsId).NotEmpty();
            RuleFor(entity => entity.VaultAccessCode).NotEmpty();
            RuleFor(entity => entity.Name).NotEmpty().MaximumLength(75).When(x => x.Name is not null);
            RuleFor(entity => entity.Description).NotEmpty().MaximumLength(255).When(x => x.Description is not null);
            RuleFor(entity => entity.PrimaryCredential).MaximumLength(2055).When(x => x.PrimaryCredential is not null); ;
            RuleFor(entity => entity.SecondaryCredential).MaximumLength(2055).When(x => x.SecondaryCredential is not null); ;
        }
    }
}
