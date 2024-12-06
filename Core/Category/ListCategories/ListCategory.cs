using Core.Credentials;
using Core.Credentials.CreateCredentials;
using Core.Credentials.ListCredentials;
using Core.User;
using Core.Vault;
using FluentResults;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Category.ListCategories
{
    public class ListCategory(ICategoryRepository categoryRepository, IUserRepository userRepository, IAuthorizationManager authorizationManager)
    {
        public async Task<IResult<List<Category>>> Execute(ListCategoriesFiltersDTO listingConfig)
        {
            var validationRules = new ListCategoriesValidator();
            ValidationResult results = validationRules.Validate(listingConfig);
            if (results.IsValid is false)
            {
                return Result.Fail<List<Category>>(Common.ToResultErrorList(results.Errors));
            }

            var userExists = await userRepository.FindByIdentifier(listingConfig.UserId);
            if (userExists.IsFailed)
            {
                return Result.Fail<List<Category>>(userExists.Errors);
            }

            User.User user = userExists.Value;
            bool callerIsResourceOwner = authorizationManager.VerifyOwnership(user.ID);
            if (!callerIsResourceOwner)
            {
                return Result.Fail<List<Category>>(Common.LACK_OWNERSHIP_ERR);
            }

            var listing = await categoryRepository.ListCategories(listingConfig);
            if (listing.IsFailed)
            {
                return Result.Fail<List<Category>>(listing.Errors);
            }

            return Result.Ok(listing.Value);
        }
    }
}
