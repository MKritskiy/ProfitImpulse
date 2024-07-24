namespace Profiles.API.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public int UserId { get; set; }
        public string? ProfileName { get; set; }
        public string? ApiKey { get; set; }
    }
}
