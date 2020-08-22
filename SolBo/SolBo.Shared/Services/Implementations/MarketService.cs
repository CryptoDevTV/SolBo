using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Services.Responses;

namespace SolBo.Shared.Services.Implementations
{
    public class MarketService : IMarketService
    {
        public FundResponse AvailableQuote(decimal fundPercentage, decimal availableQuote, int precision)
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
        public MarketResponse IsGoodToBuy(CommissionType commissionType, decimal changePriceDrop, decimal storedPriceAverage, decimal currentPrice)
        {
            if (commissionType == CommissionType.PERCENTAGE)
            {
                return new MarketResponse
                {
                    IsReadyForMarket = storedPriceAverage > currentPrice
                        ? 100 - (currentPrice / storedPriceAverage * 100) >= changePriceDrop
                        : false,
                    Changed = decimal.Round(100 - (currentPrice / storedPriceAverage * 100), 2)
                };
            }
            else
            {
                return new MarketResponse
                {
                    IsReadyForMarket = storedPriceAverage - currentPrice >= changePriceDrop,
                    Changed = storedPriceAverage - currentPrice
                };
            }
        }
        public MarketResponse IsGoodToSell(CommissionType commissionType, decimal changePriceRise, decimal storedPriceAverage, decimal currentPrice)
        {
            if (commissionType == CommissionType.PERCENTAGE)
            {
                return new MarketResponse
                {
                    IsReadyForMarket = currentPrice > storedPriceAverage
                        ? 100 - (storedPriceAverage * 100 / currentPrice) >= changePriceRise
                        : false,
                    Changed = decimal.Round(100 - (storedPriceAverage * 100 / currentPrice), 2)
                };
            }
            else
            {
                return new MarketResponse
                {
                    IsReadyForMarket = storedPriceAverage + changePriceRise >= currentPrice,
                    Changed = storedPriceAverage + changePriceRise - currentPrice
                };
            }
        }
        public MarketResponse IsStopLossReached(CommissionType commissionType, decimal changeStopLoss, decimal storedPriceAverage, decimal currentPrice)
        {
            if (commissionType == CommissionType.PERCENTAGE)
            {
                return new MarketResponse
                {
                    IsReadyForMarket = storedPriceAverage > currentPrice
                        ? 100 - (currentPrice / storedPriceAverage * 100) >= changeStopLoss
                        : false,
                    Changed = decimal.Round(100 - (currentPrice / storedPriceAverage * 100), 2)
                };
            }
            else
            {
                return new MarketResponse
                {
                    IsReadyForMarket = storedPriceAverage - changeStopLoss >= currentPrice,
                    Changed = storedPriceAverage - currentPrice
                };
            }
        }
    }
}