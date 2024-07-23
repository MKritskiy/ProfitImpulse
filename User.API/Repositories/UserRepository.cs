using Dapper;
using System.Data;
using Users.API.Database;
using Users.API.Models;

namespace Users.API.Repositories
{
    public class UserRepository : IUserRepository
    {

        public async Task<int> CreateUserAsync(User user)
        {
            string sql = @"INSERT INTO Users (email, username, password_hash, salt)
                        VALUES (@Email, @Username, @PasswordHash, @Salt)
                        RETURNING user_id";
            return await DbHelper.QueryFirstOrDefaultAsync<int>(sql, user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            string sql = "DELETE FROM Users WHERE user_id = @UserId";
            var affectedRwos = await DbHelper.ExecuteAsync(sql, new {UserId = userId});
            return affectedRwos > 0;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            string sql = @"SELECT * 
                           FROM Users 
                           WHERE email=@Email";
            return await DbHelper.QueryFirstOrDefaultAsync<User>(sql, new { Email = email }) ?? null;
        }

        public async Task<User?> GetUserByIdAsync(int userid)
        {
            string sql = @"SELECT * 
                           FROM Users 
                           WHERE user_id=@UserId";
            return await DbHelper.QueryFirstOrDefaultAsync<User>(sql, new {UserId=userid}) ?? null;
        }

        public async Task<User?> GetUserByNameAsync(string username)
        {
            string sql = @"SELECT * 
                          FROM Users 
                          WHERE username=@Username";
            return await DbHelper.QueryFirstOrDefaultAsync<User>(sql, new { Username = username }) ?? null;
        }

        public async Task<User?> GetUserByNameOrEmailAsync(string nameOrEmail)
        {
            string sql = "SELECT * FROM Users WHERE username=@NameOrEmail OR email=@NameOrEmail";
            return await DbHelper.QueryFirstOrDefaultAsync<User>(sql, new { NameOrEmail = nameOrEmail }) ?? null;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            string sql = @"UPDATE Users SET
                            email = @Email,
                            username = @Username,
                            password_hash = @PasswordHash,
                            salt = @Salt
                            WHERE user_id = @UserId";
            var affectedRows = await DbHelper.ExecuteAsync(sql, user);
            return affectedRows > 0;
        }
    }
}
