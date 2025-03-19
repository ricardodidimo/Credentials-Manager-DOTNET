using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Category.ListCategories
{
    public record ListCategoriesFiltersDTO(string UserId, int CurrentPage = 1, int PageSize = 10);
}
