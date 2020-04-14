using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SolBo.Shared.Services.Implementations
{
    public class AverageAllCalculationService : ICalculationService
    {
        public decimal CalculateAverage(IEnumerable<decimal> values)
            => values.Average();

        public bool IsGoodToBuy(int percentPriceDrop)
        {
            throw new NotImplementedException();
        }

        public bool IsGoodToSell(int percentPriceRise)
        {
            throw new NotImplementedException();
        }
    }
}