using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.API.Models;
using Users.API.Services.General;

namespace Users.API.Services.Token
{
    public class TokenGenerator : ITokenGenerator
    {
        private JwtSettings _jwtSettings;

        public TokenGenerator(IConfiguration configuration)
        {
            _jwtSettings = new JwtSettings(configuration);
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            { 
                new Claim(ClaimTypes.SerialNumber, user.UserId.ToString()) 
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: _jwtSettings.TokenLifetime,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
