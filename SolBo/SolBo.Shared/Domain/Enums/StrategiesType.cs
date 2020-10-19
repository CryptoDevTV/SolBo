using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum StrategiesType
    {
        [Description("BUY DEEP SELL HIGH")]
        BUY_DEEP_SELL_HIGH = 0,

        [Description("ROLLING PRICE")]
        ROLLING_PRICE = 1
    }
}