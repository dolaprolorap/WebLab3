using System.ComponentModel.DataAnnotations;

namespace backend.Models.DB
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [MaxLength(16)]
        [Required]
        public string UserName { get; set; }
        [MaxLength(16)]
        [Required]
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        protected User() {}

        public User(Guid guid, string name, string password, string? refreshToken = null, DateTime? refreshTokenExp = null)
        {
            UserId = guid;
            UserName = name;
            Password = password;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExp;
        }
    }
}
