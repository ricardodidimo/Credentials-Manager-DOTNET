using FluentResults;

namespace Core.User
{
    public interface IUserRepository
    {
        public Task<IResult<User>> FindByIdentifier(string uuid);
        public Task<IResult<User>> FindByEmail(string email);
        public Task<IResult<User>> CreateUser(User user);
        public Task<IResult<User>> UpdateUser(User user);
        public Task<IResult<User>> DeleteUser(User user);
    }
}
