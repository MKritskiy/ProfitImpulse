using Users.API.Services.Dto;

namespace Users.API.Services.UserService
{
    public interface IUserService
    {
        Task<AfterAuthenticationDto> Register(RegisterDto registerDto);
        Task<AfterAuthenticationDto> Login(LoginDto loginDto);
    }
}
