namespace backend.Models.API.Listener
{
    public class CreateWorkerRequest
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public int Count { get; set; }
        public string Mode { get; set; }
        public string Body { get; set; }
        public int Sleep { get; set; }
        public Guid PlotId { get; set; }
    }
}
