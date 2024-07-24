using Microsoft.AspNetCore.Mvc;
using Users.API.Dto;
using Users.API.Exceptions;
using Users.API.Services.UserService;

namespace Users.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var afterAuth = await _userService.Register(registerDto);
                return Ok(afterAuth);
            }
            catch (DuplicateEmailException e)
            {
                return Unauthorized("Email is invalid or already exists.");
            }
            catch (DuplicateUsernameException e)
            {
                return Unauthorized("Username is invalid or already exists.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest("An error occurred during registration.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var afterAuth = await _userService.Login(loginDto);
                return Ok(afterAuth);
            }
            catch (AuthorizationException e)
            {
                return Unauthorized("Invalid credentials.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return BadRequest("An error occurred during login.");
            }

        }

    }
}
