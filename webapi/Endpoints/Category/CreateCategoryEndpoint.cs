using Core.Category;
using Core.User;
using Core.Vault;
using FluentResults;
using Infrastructure.Repositories.EfCore;
using webapi.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Core.Category.CreateCategory;
using webapi.Endpoints.Vault;

namespace webapi.Endpoints.Category
{
    public class CreateCategoryEndpoint
    {
        /// <summary>
        /// Creates a new category under specific vault
        /// </summary>
        /// 
        /// <response code="201">Returns info about the created category</response>
        /// <response code="400">Data validation or resource authorization failed for creation action</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// Returns info about the created category
        /// </returns>
        /// 
        /// <example>
        /// {
        ///      "Name": "My new category!",
        ///      "UserId": "6032e9d6-5eba-42e7-3d4d-0f15e45eec4a"
        /// }
        /// </example>
        public static async Task<IResult> Execute(CreateCategoryDTO createCategoryDTO,
        IAuthorizationManagerJWT accessManager,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository)
        {
            var createUseCase = new CreateCategory(userRepository, categoryRepository, accessManager);
            IResult<CategoryModel> action = await createUseCase.Execute(createCategoryDTO);
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return CategoryResponseDTO.ToCategoryResponseDTO(action.Value).ToApiSuccess(201);
        }
    }
}
