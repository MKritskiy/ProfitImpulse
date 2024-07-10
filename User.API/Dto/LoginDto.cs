namespace Users.API.Dto
{
    public class LoginDto
    {
        public string NameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
