using System;
using System.Collections.Generic;
using System.Linq;

namespace SolBo.Shared.Services.Implementations
{
    public class AverageAllCalculationService : ICalculationService
    {
        public decimal CalculateAverage(IEnumerable<decimal> values)
            => decimal.Round(values.Average(), 2);

        public bool IsGoodToBuy(int percentPriceDrop, decimal storedPriceAverage, decimal currentPrice)
            => storedPriceAverage > currentPrice
            ? 100 - (currentPrice / storedPriceAverage * 100) >= percentPriceDrop
            : false;

        public bool IsGoodToSell(int percentPriceRise, decimal storedPriceAverage, decimal currentPrice)
        {
            if (storedPriceAverage < currentPrice)
            {
                var div = currentPrice / storedPriceAverage;

                var dec = decimal.Round(div - Math.Truncate(div), 2);

                return dec * 100 > percentPriceRise;
            }
            else
                return false;
        }
    }
}