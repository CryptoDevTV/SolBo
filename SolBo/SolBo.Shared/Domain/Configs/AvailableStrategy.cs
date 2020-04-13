namespace SolBo.Shared.Domain.Configs
{
    public class AvailableStrategy
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string StoragePath { get; set; }
        public int BidRatio { get; set; }
        public int AskRatio { get; set; }
    }
}