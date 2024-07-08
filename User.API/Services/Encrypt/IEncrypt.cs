namespace Users.API.Services.Encrypt
{
    public interface IEncrypt
    {
        string HashPassword(string password, string salt);
    }
}
