using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(16)]
        [Required]
        public string Name { get; set; }
        [MaxLength(16)]
        [Required]
        public string Password { get; set; }
    }
}
