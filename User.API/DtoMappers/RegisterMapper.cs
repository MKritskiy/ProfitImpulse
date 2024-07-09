using Users.API.Models;
using Users.API.Services.Dto;

namespace Users.API.DtoMappers
{
    public static class RegisterMapper
    {

        public static User MapRegisterDtoToUser(RegisterDto dto)
        {
            return new User()
            {
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = dto.Password
            };
        }

    }
}
