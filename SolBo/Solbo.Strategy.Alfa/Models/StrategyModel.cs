using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Strategies;
using System.Text.Json.Serialization;

namespace Solbo.Strategy.Alfa.Models
{
    public class StrategyModel : StrategyModelBase
    {
        public decimal BuyDown { get; set; }
        public decimal SellUp { get; set; }
        public int Average { get; set; }
        public decimal StopLossDown { get; set; }
        public decimal FundPercentage { get; set; }
        public bool ClearOnStartup { get; set; }
        public int StopLossPauseCycles { get; set; }
        public AverageType AverageType { get; set; }
        public SellType SellType { get; set; }
        public CommissionType CommissionType { get; set; }

        [JsonIgnore]
        public bool IsStopLossOn
            => StopLossDown > 0;
    }
}