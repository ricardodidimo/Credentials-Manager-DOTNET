global using CategoryModel = Core.Category.Category;

using Core.Category.CreateCategory;
using FluentResults;
using FluentAssertions;
using NSubstitute;
using Core.User;

namespace Tests.Core.Category
{
    public class CreateCategoryTest : CategoryTestsBase
    {
        [Fact]
        public async void Validation_fails_when_empty_name()
        {
            var input = new CreateCategoryDTO("", Guid.NewGuid().ToString());
            var action = new CreateCategory(this._userRepository, this._categoryRepository, this._authorizationManager);
            IResult<CategoryModel> res = await action.Execute(input);

            res.IsFailed.Should().BeTrue();
            res.Errors.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async void Validation_fails_when_name_exceeds_length_constraint()
        {
            var input = new CreateCategoryDTO(new string('#', 51), Guid.NewGuid().ToString());
            var action = new CreateCategory(this._userRepository, this._categoryRepository, this._authorizationManager);
            IResult<CategoryModel> res = await action.Execute(input);

            res.IsFailed.Should().BeTrue();
            res.Errors.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async void Validation_fails_when_user_id_empty()
        {
            var input = new CreateCategoryDTO(new string('#', 50), "");
            var action = new CreateCategory(this._userRepository, this._categoryRepository, this._authorizationManager);
            IResult<CategoryModel> res = await action.Execute(input);

            res.IsFailed.Should().BeTrue();
            res.Errors.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async void Validation_fails_when_user_not_found()
        {
            var input = new CreateCategoryDTO(new string('#', 50), Guid.NewGuid().ToString());
            IResult<User> errCall = Result.Fail<User>("User wasn't found!");
            this._userRepository.FindByIdentifier(Arg.Any<string>()).Returns(Task.FromResult(errCall));

            var action = new CreateCategory(this._userRepository, this._categoryRepository, this._authorizationManager);
            IResult<CategoryModel> res = await action.Execute(input);

            await this._userRepository.Received(1).FindByIdentifier(Arg.Any<string>());
            res.IsFailed.Should().BeTrue();
            res.Errors.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async void Validation_fails_when_caller_not_resouce_owner()
        {
            var input = new CreateCategoryDTO(new string('#', 50), Guid.NewGuid().ToString());
            var fakeUser = new User(Guid.NewGuid().ToString(), new string('a', 10), new string('b', 10), new string('c', 10));
            IResult<User> successCall = Result.Ok(fakeUser);
            this._userRepository.FindByIdentifier(Arg.Any<string>()).Returns(Task.FromResult(successCall));
            this._authorizationManager.VerifyOwnership(Arg.Any<string>()).Returns(false);

            var action = new CreateCategory(this._userRepository, this._categoryRepository, this._authorizationManager);
            IResult<CategoryModel> res = await action.Execute(input);

            this._authorizationManager.Received(1).VerifyOwnership(Arg.Any<string>());
            res.IsFailed.Should().BeTrue();
            res.Errors.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public async void Executes_creation_call_when_valid_inputs()
        {
            var input = new CreateCategoryDTO(new string('#', 50), Guid.NewGuid().ToString());
            var fakeUser = new User(Guid.NewGuid().ToString(), "name", "email", "password");
            IResult<User> successCall = Result.Ok(fakeUser);
            this._userRepository.FindByIdentifier(Arg.Any<string>()).Returns(Task.FromResult(successCall));
            this._authorizationManager.VerifyOwnership(Arg.Any<string>()).Returns(true);

            var fakeCategory = new CategoryModel()
            {
                ID = Guid.NewGuid().ToString(),
                Name = "Test",
                UserId = Guid.NewGuid().ToString(),
            };
            IResult<CategoryModel> createSuccess = Result.Ok(fakeCategory);
            this._categoryRepository.CreateCategory(Arg.Any<CategoryModel>()).Returns(Task.FromResult(createSuccess));

            var action = new CreateCategory(this._userRepository, this._categoryRepository, this._authorizationManager);
            IResult<CategoryModel> res = await action.Execute(input);

            await this._categoryRepository.Received(1).CreateCategory(Arg.Any<CategoryModel>());
            res.IsSuccess.Should().BeTrue();
            res.Errors.Should().BeEmpty();
        }
    }
}
