using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace backend.Models.DB
{
    public class Plot
    {
        [Key]
        public Guid PlotId { get; set; }
        [MaxLength(16)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string JsonData { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public User User { get; set; }

        public Plot(Guid guid, string name, string data, Guid userId)
        {
            PlotId = guid;
            Name = name;
            JsonData = data;
            UserId = userId;
        }
    }
}
