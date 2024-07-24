using Users.API.Models;

namespace Users.API.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userid);
        Task<User?> GetUserByNameOrEmailAsync(string nameOrEmail);
        Task<User?> GetUserByNameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
