using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum IntervalType
    {
        [Description("ONETIME")]
        ONETIME = 0,

        [Description("SECONDS")]
        SECONDS = 1,

        [Description("MINUTES")]
        MINUTES = 2,

        [Description("HOURS")]
        HOURS = 3,
    }
}