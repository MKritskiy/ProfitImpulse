using Users.API.Dto;
using Users.API.Models;

namespace Users.API.Services.UserService
{
    public interface IUserService
    {
        Task<AfterAuthenticationDto> Register(RegisterDto registerDto);
        Task<AfterAuthenticationDto> Login(LoginDto loginDto);
        Task ValidateNewUser(User user);
        Task<int> SaveNewUserAsync(User user);
    }
}
