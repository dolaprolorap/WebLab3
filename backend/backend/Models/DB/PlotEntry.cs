using System.ComponentModel.DataAnnotations;

namespace backend.Models.DB
{
    public class PlotEntry
    {
        [Key]
        public Guid EntryId { get; set; }
        [Required]
        public string Data { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Guid PlotId { get; set; }
        [Required]
        public Plot Plot { get; set; }
        protected PlotEntry() { }
        public PlotEntry(Guid guid, string data, DateTime date, Guid plotId)
        {
            EntryId = guid;
            Data = data;
            PlotId = plotId;
            Date = date;
        }
    }
}
