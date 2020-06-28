namespace SolBo.Shared.Messages.Rules
{
    public class PercentageMessage
    {
        public decimal Change { get; set; }
        public bool PriceReached { get; set; }
        public bool IsReady { get; set; }
        public decimal AvailableFund { get; set; }
    }
}