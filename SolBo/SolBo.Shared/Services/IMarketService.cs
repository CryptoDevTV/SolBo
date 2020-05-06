using SolBo.Shared.Services.Responses;

namespace SolBo.Shared.Services
{
    public interface IMarketService
    {
        MarketResponse IsGoodToBuy(int percentPriceDrop, decimal storedPriceAverage, decimal currentPrice);
        MarketResponse IsGoodToSell(int percentPriceRise, decimal storedPriceAverage, decimal currentPrice);
    }
}