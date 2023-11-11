namespace backend.Models.API
{
    public class PlotResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JsonData { get; set; }
        public string UserName { get; set; }
    }
}
