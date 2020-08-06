using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum AverageType
    {
        [Description("WithCurrent")]
        WITH_CURRENT = 0,

        [Description("WithoutCurrent")]
        WITHOUT_CURRENT = 1
    }
}