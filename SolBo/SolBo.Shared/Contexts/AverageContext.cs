using SolBo.Shared.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SolBo.Shared.Contexts
{
    public class AverageContext
    {
        public static decimal Average(AverageType averageType, IEnumerable<decimal> values, int round, int lastToTake = 0)
        {
            var priceValues = values;

            if (averageType == AverageType.WITHOUT_CURRENT && values.Count() > 1)
                priceValues = values.SkipLast(1);

            decimal result = lastToTake == 0
                ? priceValues.Average()
                : priceValues.Count() > lastToTake ? priceValues.TakeLast(lastToTake).Average() : priceValues.Average();

            return decimal.Round(result, round);
        }
    }
}