using Microsoft.AspNetCore.Http.HttpResults;
using Users.API.Exceptions;
using Users.API.Models;
using Users.API.Repositories;
using Users.API.Services.Dto;
using Users.API.Services.Encrypt;
using Users.API.Services.Token;

namespace Users.API.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypt _encrypt;
        private readonly ITokenGenerator _tokenGenerator;


        public UserService(IUserRepository userRepository, IEncrypt encrypt, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _encrypt = encrypt;
            _tokenGenerator = tokenGenerator;
        }


        public async Task<AfterAuthenticationDto> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByNameOrEmailAsync(loginDto.NameOrEmail);


            if (user != null && user.PasswordHash == _encrypt.HashPassword(loginDto.Password, user.Salt))
            {
                int id = user.UserId;
                var token = _tokenGenerator.GenerateToken(user);

                return new AfterAuthenticationDto { Token = token, Id = id };
            }

            throw new AuthorizationException();
        }

        public async Task<AfterAuthenticationDto> Register(RegisterDto registerDto)
        {

            User user = new User();

            user.Username = registerDto.Username;

            user.Email = registerDto.Email;

            await ValidateEmail(user.Email);
            await ValidateLogin(user.Username);

            user.Salt = Guid.NewGuid().ToString();
            user.PasswordHash = _encrypt.HashPassword(registerDto.Password, user.Salt);

            int id = await _userRepository.CreateUserAsync(user);

            var token = _tokenGenerator.GenerateToken(user);

            return new AfterAuthenticationDto { Token = token, Id = id };

        }
        

        private async Task ValidateLogin(string username)
        {
            var user = await _userRepository.GetUserByNameOrEmailAsync(username);
            if (user != null)
                throw new DuplicateUsernameException();
        }

        private async Task ValidateEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user != null)
                throw new DuplicateEmailException();
        }
    }
}
