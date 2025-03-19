using Core.User;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;

namespace Core.Vault.CreateVault
{
    public class CreateVault(IUserRepository userRepository, IVaultRepository vaultRepository, IAuthorizationManager authorizationManager)
    {
        public async Task<IResult<Vault>> Execute(CreateVaultDTO vaultDTO)
        {
            var validationRules = new CreateVaultValidator();
            ValidationResult results = validationRules.Validate(vaultDTO);
            if (results.IsValid is false)
            {
                return Result.Fail<Vault>(Common.ToResultErrorList(results.Errors));
            }

            IResult<User.User> vaultOwnerExists = await userRepository.FindByIdentifier(vaultDTO.UserID);
            if (vaultOwnerExists.IsFailed)
            {
                return Result.Fail<Vault>(vaultOwnerExists.Errors);
            }

            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(vaultOwnerExists.Value.ID);
            if (!callerIsResourceOwner)
            {
                return Result.Fail<Vault>(Common.LACK_OWNERSHIP_ERR);
            }

            string UUID = Guid.NewGuid().ToString();
            string hashAccessCode = new PasswordHasher<CreateVaultDTO>().HashPassword(vaultDTO, vaultDTO.AccessCode);
            var vault = new Vault { 
                ID = UUID,
                Name = vaultDTO.Name,
                Description = vaultDTO.Description,
                AccessCode = hashAccessCode,
                User = vaultOwnerExists.Value,
                UserId = vaultOwnerExists.Value.ID,
            };

            IResult<Vault> created = await vaultRepository.CreateVault(vault);
            return created;
        }
    }
}
