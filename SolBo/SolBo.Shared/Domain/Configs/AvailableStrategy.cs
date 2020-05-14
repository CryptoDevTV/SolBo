namespace SolBo.Shared.Domain.Configs
{
    public class AvailableStrategy
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string StoragePath { get; set; }
        public int BuyPercentageDown { get; set; }
        public int SellPercentageUp { get; set; }
        public int Average { get; set; }
        public int Ticker { get; set; }
        public int StopLossPercentageDown { get; set; }
        public int FundPercentage { get; set; }
    }
}