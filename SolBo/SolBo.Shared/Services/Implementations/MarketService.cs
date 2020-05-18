using SolBo.Shared.Services.Responses;

namespace SolBo.Shared.Services.Implementations
{
    public class MarketService : IMarketService
    {
        public FundResponse AvailableQuote(int fundPercentage, decimal availableQuote, int precision)
        {
            var result = new FundResponse();

            if (fundPercentage > 0 && fundPercentage < 100)
            {
                result.QuoteAssetToTrade = decimal.Round(availableQuote * fundPercentage / 100, precision);
            }
            else
                result.QuoteAssetToTrade = availableQuote;

            return result;
        }
        public MarketResponse IsGoodToBuy(int percentPriceDrop, decimal storedPriceAverage, decimal currentPrice)
        {
            return new MarketResponse
            {
                IsReadyForMarket = storedPriceAverage > currentPrice
                    ? 100 - (currentPrice / storedPriceAverage * 100) >= percentPriceDrop
                    : false,
                PercentChanged = decimal.Round(100 - (currentPrice / storedPriceAverage * 100), 2)
            };
        }
        public MarketResponse IsGoodToSell(int percentPriceRise, decimal storedPriceAverage, decimal currentPrice)
        {
            return new MarketResponse
            {
                IsReadyForMarket = currentPrice > storedPriceAverage
                    ? 100 - ((storedPriceAverage * 100) / currentPrice) >= percentPriceRise
                    : false,
                PercentChanged = decimal.Round(100 - ((storedPriceAverage * 100) / currentPrice), 2)
            };
        }
        public MarketResponse IsStopLossReached(int percentStopLoss, decimal storedPriceAverage, decimal currentPrice)
        {
            return new MarketResponse
            {
                IsReadyForMarket = storedPriceAverage > currentPrice
                    ? 100 - (currentPrice / storedPriceAverage * 100) >= percentStopLoss
                    : false,
                PercentChanged = decimal.Round(100 - (currentPrice / storedPriceAverage * 100), 2)
            };
        }
    }
}