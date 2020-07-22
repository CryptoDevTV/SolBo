namespace SolBo.Shared.Domain.Configs
{
    public class App
    {
        public string Version { get; set; }
        public string FileName { get; set; }
        public int IntervalInMinutes { get; set; }
        public Notifications Notifications { get; set; }
    }
}