using Core.Category;
using Core.User;
using Core;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.Category
{
    public class CategoryTestsBase
    {
        internal IUserRepository _userRepository = Substitute.For<IUserRepository>();
        internal ICategoryRepository _categoryRepository = Substitute.For<ICategoryRepository>();
        internal IAuthorizationManager _authorizationManager = Substitute.For<IAuthorizationManager>();
    }
}
