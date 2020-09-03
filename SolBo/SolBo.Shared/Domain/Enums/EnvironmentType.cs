using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum EnvironmentType
    {
        [Description("TEST")]
        TEST = 0,

        [Description("PRODUCTION")]
        PRODUCTION = 1
    }
}