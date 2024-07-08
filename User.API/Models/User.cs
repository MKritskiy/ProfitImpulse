using System.ComponentModel.DataAnnotations.Schema;

namespace Users.API.Models
{
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        [Column("password_hash")]
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!;
    }
}
