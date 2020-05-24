using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum TickerType
    {
        [Description("CURRENTPRICE")]
        CURRENTPRICE = 0,

        [Description("CURRENTAVERAGEPRICE")]
        CURRENTAVERAGEPRICE = 1
    }
}