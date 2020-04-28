using System.Collections.Generic;
using System.Linq;

namespace SolBo.Shared.Contexts
{
    public class AverageContext
    {
        public static decimal Average(IEnumerable<decimal> values, int lastToTake = 0)
        {
            decimal result = lastToTake == 0
                ? values.Average()
                : values.Count() > lastToTake ? values.TakeLast(lastToTake).Average() : values.Average();

            return decimal.Round(result, 2);
        }
    }
}