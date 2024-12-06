using Core.Category.CreateCategory;
using Core.User;
using Core.Vault;
using FluentResults;
using webapi.Helpers;
using Core.Category.ListCategories;
using Infrastructure.Repositories.EfCore;
using Microsoft.AspNetCore.Mvc;
using Core.Category;
using Core.Category.UpdateCategory;
using Core;
using webapi.Endpoints.Vault;

namespace webapi.Endpoints.Category
{
    /// <summary>
    /// Updates selected info about an existing category
    /// </summary>
    /// 
    /// <response code="200">Returns info about the updated category</response>
    /// <response code="400">Data validation or resource authorization failed for update action</response>
    /// <response code="401">Unauthenticated request</response>
    /// 
    /// <returns>
    /// Returns info about the updated category
    /// </returns>
    /// 
    /// <example>
    /// {
    ///     "CategoryId": "9232e9d6-5eba-c2e7-334d-0f39035eec4a",
    ///     "Name": "My updated category!",
    /// }
    /// </example>
    public class PatchCategoryEndpoint
    {
        public static async Task<IResult> Execute(
        UpdateCategoryDTO updateDTO,
        IAuthorizationManager accessManager,
        ICategoryRepository categoryRepository)
        {
            var updateUseCase = new UpdateCategory(accessManager, categoryRepository);
            IResult<CategoryModel> action = await updateUseCase.Execute(updateDTO);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return CategoryResponseDTO.ToCategoryResponseDTO(action.Value).ToApiSuccess();
        }
    }
}
