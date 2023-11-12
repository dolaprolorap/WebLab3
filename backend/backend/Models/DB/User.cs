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

        protected User() {}

        public User(Guid guid, string name, string password)
        {
            UserId = guid;
            UserName = name;
            Password = password;
        }
    }
}
