using System;

namespace SolBo.Shared.Utils
{
    public static class MathUtils
    {
        public static decimal QuantityFloor(decimal number, int premission = 8)
        {
            int num = (int)Math.Pow(10, premission);
            return Math.Floor(number * num) / num;
        }

        public static decimal PriceFloor(decimal number)
            => Math.Floor(number * 100000000) / 100000000;
    }
}