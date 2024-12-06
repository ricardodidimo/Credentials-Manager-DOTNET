using FluentResults;
using FluentValidation.Results;

namespace Core.Category.UpdateCategory
{
    public class UpdateCategory(
        IAuthorizationManager authorizationManager,
        ICategoryRepository categoryRepository)
    {
        public async Task<IResult<Category>> Execute(UpdateCategoryDTO updateDTO)
        {
            var validationRules = new UpdateCategoryValidator();
            ValidationResult results = validationRules.Validate(updateDTO);
            if (results.IsValid is false)
            {
                return Result.Fail<Category>(Common.ToResultErrorList(results.Errors));
            }

            IResult<Category> entityExists = await categoryRepository.FindByIdentifier(updateDTO.CategoryId);
            if (entityExists.IsFailed) {
                return Result.Fail<Category>(Common.CATEGORY_REFERENCE_NOT_FOUND_ERR);
            }

            Category category = entityExists.Value;
            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(category.User!.ID);
            if (!callerIsResourceOwner) {
                return Result.Fail<Category>(Common.LACK_OWNERSHIP_ERR);
            }

            if (updateDTO.Name is not null) category.Name = updateDTO.Name;           
            return await categoryRepository.UpdateCategory(category);
        }
    }
    }
