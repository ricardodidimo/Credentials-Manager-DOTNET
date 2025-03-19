using Core.Category.ListCategories;
using FluentResults;

namespace Core.Category
{
    public interface ICategoryRepository
    {
        public Task<IResult<List<Category>>> ListCategories(ListCategoriesFiltersDTO config);
        public Task<IResult<Category>> FindByIdentifier(string uuid);
        public Task<IResult<Category>> CreateCategory(Category category);
        public Task<IResult<Category>> UpdateCategory(Category category);
        public Task<IResult<Category>> DeleteCategory(Category category);
    }
}
