using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum SellType
    {
        [Description("FROM BOUGHT VALUE")]
        FROM_VALUE = 0,

        [Description("FROM AVERAGE VALUE")]
        FROM_AVERAGE_VALUE = 1
    }
}