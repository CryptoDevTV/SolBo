namespace SolBo.Shared.Domain.Configs
{
    public class AvailableStrategy
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal BuyPercentageDown { get; set; }
        public decimal SellPercentageUp { get; set; }
        public int Average { get; set; }
        public int Ticker { get; set; }
        public decimal StopLossPercentageDown { get; set; }
        public decimal FundPercentage { get; set; }
        public int StopLossType { get; set; }
    }
}