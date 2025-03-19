using FluentResults;
using webapi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Core.Category;
using Core;
using Core.Category.DeleteCategory;
using webapi.Endpoints.Vault;

namespace webapi.Endpoints.Category
{
    public class DeleteCategoryEndpoint
    {
        /// <summary>
        /// Deletes a specific category
        /// </summary>
        /// 
        /// <param name="id">The unique identifier of the category</param>
        /// 
        /// <response code="204">Returns no data yet indicates a successful delete</response>
        /// <response code="400">If the category is not found Or resource authorization failed for delete action</response>
        /// <response code="401">Unauthenticated request</response>
        /// 
        /// <returns>
        /// There is no return for this action
        /// </returns>
        public static async Task<IResult> Execute([FromQuery] string id, IAuthorizationManager accessManager, ICategoryRepository repository)
        {
            var deleteUseCase = new DeleteCategory(accessManager, repository);
            IResult<CategoryModel> action = await deleteUseCase.Execute(new DeleteCategoryDTO(id));
            if (action.IsFailed)
            {
                return action.Errors.ToApiError(400);
            }

            return action.Value.ToApiNoContent();
        }
    }
}
