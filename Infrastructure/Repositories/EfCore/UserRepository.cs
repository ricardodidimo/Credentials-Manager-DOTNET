using Core.User;
using FluentResults;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EfCore
{
    public class UserRepository(ApplicationDbContext _context) : IUserRepository
    {
        public const string USER_NOT_FOUND = "Unable to find user";

        public async Task<IResult<User>> FindByIdentifier(string identifier)
        {
           var result = await _context.Users.Where(u => u.ID.Equals(identifier)).FirstOrDefaultAsync();
           if (result is null)
            {   
                return Result.Fail<User>(USER_NOT_FOUND);
            }

           return Result.Ok(result);
        }

        public async Task<IResult<User>> FindByEmail(string email)
        {
            var result = await _context.Users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();
            if (result is null)
            {
                return Result.Fail<User>(USER_NOT_FOUND);
            }

            return Result.Ok(result);
        }

        public async Task<IResult<User>> CreateUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            var inserts =  await _context.SaveChangesAsync();
            if (inserts is not 1)
            {
                return Result.Fail<User>(ErrorMessages.UNABLE_CREATE);   
            }

            return Result.Ok(user);
        }

        public async Task<IResult<User>> UpdateUser(User user)
        {
            _context.Users.Update(user);
            var updates = await _context.SaveChangesAsync();
            if (updates is not 1)
            {
                return Result.Fail<User>(ErrorMessages.UNABLE_UPDATE);
            }

            return Result.Ok(user);
        }

        public async Task<IResult<User>> DeleteUser(User user)
        {
            _context.Users.Remove(user);
            var deletes = await _context.SaveChangesAsync();
            if (deletes is not 1)
            {
                return Result.Fail<User>(ErrorMessages.UNABLE_DELETE);
            }

            return Result.Ok(user);
        }
    }
}
