using SolBo.Shared.Domain.Enums;

namespace SolBo.Shared.Domain.Configs
{
    public class AvailableStrategy
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal BuyPercentageDown { get; set; }
        public decimal SellPercentageUp { get; set; }
        public int Average { get; set; }
        public TickerType TickerType { get; set; }
        public decimal StopLossPercentageDown { get; set; }
        public decimal FundPercentage { get; set; }
        public StopLossType StopLossType { get; set; }
        public bool ClearOnStartup { get; set; }
        public int StopLossPauseCycles { get; set; }
        public AverageType AverageType { get; set; }
    }
}