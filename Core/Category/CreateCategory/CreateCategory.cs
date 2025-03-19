using Core.Credentials.CreateCredentials;
using Core.User;
using Core.Vault;
using FluentResults;
using FluentValidation.Results;

namespace Core.Category.CreateCategory
{
    public class CreateCategory(
        IUserRepository userRepository,
        ICategoryRepository categoryRepository, 
        IAuthorizationManager authorizationManager
    ) {
        public async Task<IResult<Category>> Execute(CreateCategoryDTO createDTO)
        {
            var validationRules = new CreateCategoryValidator();
            ValidationResult results = validationRules.Validate(createDTO);
            if (results.IsValid is false)
            {
                return Result.Fail<Category>(Common.ToResultErrorList(results.Errors));
            }

            IResult<User.User> userExists = await userRepository.FindByIdentifier(createDTO.UserId);
            if (userExists.IsFailed)
            {
                return Result.Fail<Category>(userExists.Errors);
            }

            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(userExists.Value.ID);
            if (!callerIsResourceOwner)
            {
                return Result.Fail<Category>(Common.LACK_OWNERSHIP_ERR);
            }

            string UUID = Guid.NewGuid().ToString();
            var category = new Category
            {
                ID = UUID,
                Name = createDTO.Name,
                UserId = userExists.Value.ID,
                User = userExists.Value,
            };

            return await categoryRepository.CreateCategory(category);
        }
    }
}
