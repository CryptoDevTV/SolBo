using System;

namespace SolBo.Shared.Extensions
{
    public static class DecimalExt
    {
        public static decimal ToKucoinRound(this decimal dec)
            => Math.Truncate(dec * 100000) / 100000;
    }
}