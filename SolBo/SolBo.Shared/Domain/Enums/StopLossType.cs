using System.ComponentModel;

namespace SolBo.Shared.Domain.Enums
{
    public enum StopLossType
    {
        [Description("MARKETSELL")]
        MARKETSELL = 0,

        [Description("STOPLOSSLIMIT")]
        STOPLOSSLIMIT = 1
    }
}