namespace SolBo.Shared.Services
{
    public interface IMarketService
    {
        bool IsGoodToBuy(int percentPriceDrop, decimal storedPriceAverage, decimal currentPrice);
        bool IsGoodToSell(int percentPriceRise, decimal storedPriceAverage, decimal currentPrice);
    }
}