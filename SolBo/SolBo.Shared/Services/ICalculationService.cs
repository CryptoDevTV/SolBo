using System.Collections.Generic;

namespace SolBo.Shared.Services
{
    public interface ICalculationService
    {
        decimal CalculateAverage(IEnumerable<decimal> values);
        bool IsGoodToBuy(int percentPriceDrop, decimal storedPriceAverage, decimal currentPrice);
        bool IsGoodToSell(int percentPriceRise, decimal storedPriceAverage, decimal currentPrice);
    }
}