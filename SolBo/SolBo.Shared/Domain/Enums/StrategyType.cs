using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum StrategyType
    {
        [Description("BuyDeepSellHigh")]
        BUY_DEEP_SELL_HIGH = 0,

        [Description("RollingPrice")]
        ROLLING_PRICE = 1
    }
}