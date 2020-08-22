using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum CommissionType
    {
        [Description("Value")]
        VALUE = 0,

        [Description("Percentage")]
        PERCENTAGE = 1
    }
}