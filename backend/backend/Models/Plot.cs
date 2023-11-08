using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace backend.Models
{
    public class Plot
    {
        [Key]
        public int PlotId { get; set; }
        [MaxLength(16)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string JsonData { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; }
    }
}
