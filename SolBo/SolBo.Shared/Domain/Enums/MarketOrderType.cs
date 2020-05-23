using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum MarketOrderType
    {
        [Description("BUY")]
        BUYING = 0,

        [Description("SELL")]
        SELLING = 1,

        [Description("STOPLOSS")]
        STOPLOSS = 2
    }
}