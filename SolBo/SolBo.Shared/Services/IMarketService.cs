using SolBo.Shared.Services.Responses;

namespace SolBo.Shared.Services
{
    public interface IMarketService
    {
        MarketResponse IsGoodToBuy(decimal percentPriceDrop, decimal storedPriceAverage, decimal currentPrice);
        MarketResponse IsGoodToSell(decimal percentPriceRise, decimal storedPriceAverage, decimal currentPrice);
        MarketResponse IsStopLossReached(decimal percentStopLoss, decimal storedPriceAverage, decimal currentPrice);
        FundResponse AvailableQuote(decimal fundPercentage, decimal availableQuote, int precision);
    }
}