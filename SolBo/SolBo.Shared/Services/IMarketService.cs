using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Services.Responses;
using System.Collections.Generic;

namespace SolBo.Shared.Services
{
    public interface IMarketService
    {
        MarketResponse IsGoodToBuy(CommissionType commissionType, decimal changePriceDrop, decimal storedPriceAverage, decimal currentPrice);
        MarketResponse IsGoodToSell(CommissionType commissionType, decimal changePriceRise, decimal storedPriceAverage, decimal currentPrice);
        MarketResponse IsStopLossReached(CommissionType commissionType, decimal changeStopLoss, decimal storedPriceAverage, decimal currentPrice);
        FundResponse AvailableQuote(decimal fundPercentage, decimal availableQuote, int precision);
        decimal Average(AverageType averageType, IEnumerable<decimal> values, int round, int lastToTake = 0);
    }
}