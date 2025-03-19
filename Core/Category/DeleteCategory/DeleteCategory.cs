using Core.Category.UpdateCategory;
using Core.Credentials;
using Core.User;
using Core.Vault;
using Core.Vault.CreateVault;
using FluentResults;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Category.DeleteCategory
{

    public class DeleteCategory(IAuthorizationManager authorizationManager, ICategoryRepository repository)
    {
        public async Task<IResult<Category>> Execute(DeleteCategoryDTO deleteDTO)
        {
            var validationRules = new DeleteCategoryValidator();
            ValidationResult results = validationRules.Validate(deleteDTO);
            if (results.IsValid is false)
            {
                return Result.Fail<Category>(Common.ToResultErrorList(results.Errors));
            }

            IResult<Category> categoryExists = await repository.FindByIdentifier(deleteDTO.CategoryId);
            if (categoryExists.IsFailed) {
                return Result.Fail<Category>(Common.CATEGORY_REFERENCE_NOT_FOUND_ERR);
            }

            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(categoryExists.Value.User!.ID);
            if (!callerIsResourceOwner) {
                return Result.Fail<Category>(Common.LACK_OWNERSHIP_ERR);
            }

            return await repository.DeleteCategory(categoryExists.Value);
        }
    }
    }
