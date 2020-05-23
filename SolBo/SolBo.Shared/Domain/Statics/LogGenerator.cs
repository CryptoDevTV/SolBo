using Binance.Net.Objects;
using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Extensions;
using SolBo.Shared.Services.Responses;
using System;

namespace SolBo.Shared.Domain.Statics
{
    public static class LogGenerator
    {
        public static string ErrorTicker => "Bad ticker definition";

        public static string ValidationSuccess(string ruleAttribute)
            => $"Validation SUCCESS => {ruleAttribute}";

        public static string ValidationError(string ruleAttribute, string attributeValue)
            => $"Validation ERROR => {ruleAttribute} => Value => {attributeValue} => BAD";

        public static string SaveSuccess => "Save SUCCESS";
        public static string SaveError => "Save ERROR";

        public static string SequenceSuccess(string sequenceName, string attribute)
            => $"{sequenceName} SUCCESS => {attribute}";
        public static string SequenceError(string sequenceName, string attribute)
            => $"{sequenceName} ERROR => {attribute}";
        public static string SequenceException(string sequenceName, Exception e)
            => $"{sequenceName} EXCEPTION => {e.GetFullMessage()}";
















        public static string CurrentPrice(AvailableStrategy availableStrategy, decimal price, decimal availableQuote)
            => $"Current price ({availableStrategy.Ticker}) for {availableStrategy.Symbol} is {price}. Available to trade: {availableQuote} ({availableStrategy.FundPercentage}%)";

        public static string AveragePrice(AvailableStrategy availableStrategy, decimal storedPriceAverage)
            => $"Average price for last {availableStrategy.Average} is {storedPriceAverage}";

        public static string StopLossOrder(MarketResponse marketResponse)
            => $"Stop Loss order ({marketResponse.IsReadyForMarket}), price change ({marketResponse.PercentChanged}%)";

        public static string StopLossOrderReady(decimal price, MarketResponse marketResponse, AvailableStrategy availableStrategy)
            => $"Price ({price}) dropped ({marketResponse.PercentChanged}%) > ({availableStrategy.StopLossPercentageDown}%), selling all {availableStrategy.Symbol}";

        public static string StopLossTest => "Stop Loss in test mode";

        public static string StopLossResultStart(long orderId)
            => $"(STOP LOSS) START selling order ({orderId})";

        public static string StopLossResult(BinanceOrderTrade item)
            => $"Order filled with Quantity ({item.Quantity}), Price ({item.Price}), Commission ({item.Commission} {item.CommissionAsset})";

        public static string StopLossResultEnd(long orderId)
            => $"(STOP LOSS) END selling order ({orderId})";

        public static string SellOrder(MarketResponse marketResponse)
            => $"Sell order ({marketResponse.IsReadyForMarket}), price change ({marketResponse.PercentChanged}%)";

        public static string SellOrderReady(decimal price, MarketResponse marketResponse, AvailableStrategy availableStrategy)
            => $"Price ({price}) increased ({marketResponse.PercentChanged}%) > ({availableStrategy.SellPercentageUp}%), selling {availableStrategy.Symbol}";

        public static string SellTest => "Sold in test mode";

        public static string SellResultStart(long orderId)
            => $"START selling order ({orderId})";

        public static string SellResult(BinanceOrderTrade item)
            => $"Order filled with Quantity ({item.Quantity}), Price ({item.Price}), Commission ({item.Commission} {item.CommissionAsset})";

        public static string SellResultEnd(long orderId)
            => $"END selling order ({orderId})";

        public static string BuyOrder(MarketResponse marketResponse)
            => $"Buy order ({marketResponse.IsReadyForMarket}), price change ({marketResponse.PercentChanged}%)";

        public static string BuyOrderReady(decimal price, MarketResponse marketResponse, AvailableStrategy availableStrategy)
            => $"Price ({price}) dropped ({marketResponse.PercentChanged}%) < ({availableStrategy.BuyPercentageDown}%), buying {availableStrategy.Symbol}";

        public static string BuyTest => "Buy in test mode";

        public static string BuyResultStart(long orderId)
            => $"START buying order ({orderId})";

        public static string BuyResult(BinanceOrderTrade item)
            => $"Order filled with Quantity ({item.Quantity}), Price ({item.Price}), Commission ({item.Commission} {item.CommissionAsset})";

        public static string BuyResultEnd(long orderId)
            => $"END buying order ({orderId})";

        public static string WarnSymbol(string symbol)
            => $"Provided symbol {symbol} is not supported on this exchange";

        public static string WarnKeys
            => $"Provided apikey or/and apisecret are not correct";

        public static string WarnStrategy
            => $"Provided activeid for strategy is not correct";

        public static string WarnFilterMinNotional(string quoteAsset, decimal availableQuote, decimal min)
            => $"Not enough {quoteAsset} ({availableQuote}), needed at least ({min})";
    }
}