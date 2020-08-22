using SolBo.Shared.Domain.Enums;

namespace SolBo.Shared.Domain.Configs
{
    public class AvailableStrategy
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal BuyDown { get; set; }
        public decimal SellUp { get; set; }
        public int Average { get; set; }
        public TickerType TickerType { get; set; }
        public decimal StopLossDown { get; set; }
        public decimal FundPercentage { get; set; }
        public StopLossType StopLossType { get; set; }
        public bool ClearOnStartup { get; set; }
        public int StopLossPauseCycles { get; set; }
        public AverageType AverageType { get; set; }
        public SellType SellType { get; set; }
        public CommissionType CommissionType { get; set; }
    }
}