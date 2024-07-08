using Users.API.Models;

namespace Users.API.Services.Token
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
