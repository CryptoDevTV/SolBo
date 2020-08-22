using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum SellType
    {
        [Description("FromValue")]
        FROM_VALUE = 0,

        [Description("FromAverageValue")]
        FROM_AVERAGE_VALUE = 1
    }
}