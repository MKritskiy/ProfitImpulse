namespace Users.API.Services.General
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int TokenLifetimeMinutes { get; set; }

        public JwtSettings() { }

        public JwtSettings(string issuer, string audience, string key, int tokenLifetimeMinutes)
        {
            Issuer = issuer;
            Audience = audience;
            Key = key;
            TokenLifetimeMinutes = tokenLifetimeMinutes;
        }
    }
}
