using System.Collections.Generic;

namespace SolBo.Shared.Services
{
    public interface ICalculationService
    {
        decimal CalculateAverage(IEnumerable<decimal> values);
        bool IsGoodToBuy(int percentPriceDrop);
        bool IsGoodToSell(int percentPriceRise);
    }
}