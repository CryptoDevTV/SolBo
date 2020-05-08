using Binance.Net.Objects;
using System;

namespace SolBo.Shared.Utils
{
    public static class MarketUtils
    {
        public static decimal ClampQuantity(BinanceSymbol symbolInfo, decimal quantity)
        {
            quantity -= quantity % symbolInfo.LotSizeFilter.StepSize;
            quantity = Math.Min(symbolInfo.LotSizeFilter.MaxQuantity, quantity);
            quantity = Math.Max(symbolInfo.LotSizeFilter.MinQuantity, quantity);
            quantity = MathUtils.QuantityFloor(quantity, symbolInfo.BaseAssetPrecision);

            return quantity;
        }

        public static decimal ClampPrice(BinanceSymbol symbolInfo, decimal priceIn, decimal weightedAverage)
        {
            decimal price = priceIn - priceIn % symbolInfo.PriceFilter.TickSize;

            if (weightedAverage == 0)
                weightedAverage = price;

            if (symbolInfo.PricePercentFilter.MultiplierUp != 0)
                price = Math.Min(symbolInfo.PricePercentFilter.MultiplierUp * weightedAverage, price);

            if (symbolInfo.PricePercentFilter.MultiplierDown != 0)
                price = Math.Max(symbolInfo.PricePercentFilter.MultiplierDown * weightedAverage, price);

            if (symbolInfo.PriceFilter.MaxPrice != 0)
                price = Math.Min(symbolInfo.PriceFilter.MaxPrice, price);

            if (symbolInfo.PriceFilter.MinPrice != 0)
                price = Math.Max(symbolInfo.PriceFilter.MinPrice, price);

            price = MathUtils.PriceFloor(price);

            return price;
        }
    }
}