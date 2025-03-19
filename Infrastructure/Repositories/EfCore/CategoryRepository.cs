using Core.Category;
using Core.Category.ListCategories;
using FluentResults;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EfCore
{
    public class CategoryRepository(ApplicationDbContext _context) : ICategoryRepository
    {
        public const string CATEGORY_NOT_FOUND = "Unable to find category";
        public async Task<IResult<List<Category>>> ListCategories(ListCategoriesFiltersDTO config)
        {
            IQueryable<Category> query = ConstructFilteredQuery(config);
            var result = await query.Skip((config.CurrentPage - 1) * config.PageSize).Take(config.PageSize).ToListAsync();
            return Result.Ok(result);
        }

        private IQueryable<Category> ConstructFilteredQuery(ListCategoriesFiltersDTO config)
        {
            IQueryable<Category> query = _context.Categories;
            if (!string.IsNullOrEmpty(config.UserId)) query = query.Where(c => c.UserId.Equals(config.UserId));

            return query;
        }

        public async Task<IResult<Category>> FindByIdentifier(string uuid)
        {
            var result = await _context.Categories.Where(c => c.ID.Equals(uuid)).Include(c => c.User).FirstOrDefaultAsync();
            if (result is null)
            {
                return Result.Fail<Category>(CATEGORY_NOT_FOUND);
            }

            return Result.Ok(result);
        }

        public async Task<IResult<Category>> CreateCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            var inserts = await _context.SaveChangesAsync();
            if (inserts is not 1)
            {
                return Result.Fail<Category>(ErrorMessages.UNABLE_CREATE);
            }

            return Result.Ok(category);
        }

        public async Task<IResult<Category>> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            var updates = await _context.SaveChangesAsync();
            if (updates is not 1)
            {
                return Result.Fail<Category>(ErrorMessages.UNABLE_UPDATE);
            }

            return Result.Ok(category);
        }

        public async Task<IResult<Category>> DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            var deletes = await _context.SaveChangesAsync();
            if (deletes is not 1)
            {
                return Result.Fail<Category>(ErrorMessages.UNABLE_DELETE);
            }

            return Result.Ok(category);
        }
    }
}
