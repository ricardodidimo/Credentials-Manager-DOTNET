using Core.Category.CreateCategory;
using Core.User;
using Core.Vault;
using FluentResults;
using webapi.Helpers;
using Core.Category.ListCategories;
using Infrastructure.Repositories.EfCore;
using Microsoft.AspNetCore.Mvc;
using Core.Category;
using webapi.Endpoints.Vault;

namespace webapi.Endpoints.Category
{
    public class ListCategoriesEndpoint
    {
        /// <summary>
        /// List categories for a specific user
        /// </summary>
        /// 
        /// <param name="userId">The unique identifier of the targeted user</param>
        /// <param name="page">Numeric identifier of the targeted page for listing</param>
        /// <param name="pageSize">Optional parameter that defines a page's length</param>
        ///         
        /// <response code="200">Returns a collection of category</response>
        /// <response code="400">General fail on processing request</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// Returns a list of categories
        /// </returns>
        public static async Task<IResult> Execute([FromQuery] string userId,
            [FromQuery] int page, [FromQuery] int? pageSize,
            IAuthorizationManagerJWT accessManager, ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            var listUseCase = new ListCategory(categoryRepository, userRepository, accessManager);
            var listFilters = new ListCategoriesFiltersDTO(userId, page, pageSize ?? 1);
            IResult<List<CategoryModel>> action = await listUseCase.Execute(listFilters);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            List<CategoryResponseDTO> response = action.Value.Select(x => CategoryResponseDTO.ToCategoryResponseDTO(x)).ToList();
            return response.ToApiSuccess();
        }
    }
}
