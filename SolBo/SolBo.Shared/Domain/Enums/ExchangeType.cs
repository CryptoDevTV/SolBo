using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum ExchangeType
    {
        [Description("BINANCE")]
        Binance = 0,

        [Description("KUCOIN")]
        KuCoin = 1,

        [Description("HUOBI")]
        Huobi = 2,

        [Description("KRAKEN")]
        Kraken = 3
    }
}