namespace Users.API.Services.General
{
    public class JwtSettings
    {
        public string Issuer { get; }
        public string Audience { get; }
        public string Key { get; }
        public DateTime TokenLifetime { get; }

        public JwtSettings(IConfiguration configuration)
        {
            Issuer = configuration.GetValue<string>("JwtSettings:Issuer") ?? "your_issuer";
            Audience = configuration.GetValue<string>("JwtSettings:Audience") ?? "your_audience";
            Key = configuration.GetValue<string>("JwtSettings:Key") ?? "your_secret_key";
            TokenLifetime = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JwtSettings:TokenLifetimeMinutes"));
        }
    }
}
