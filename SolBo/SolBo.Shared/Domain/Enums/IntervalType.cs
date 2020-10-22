using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum IntervalType
    {
        [Description("SECONDS")]
        SECONDS = 0,

        [Description("MINUTES")]
        MINUTES = 1,

        [Description("HOURS")]
        HOURS = 2,
    }
}