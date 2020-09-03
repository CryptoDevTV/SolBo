using Binance.Net.Objects.Spot.SpotData;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Domain.Statics
{
    public static class LogGenerator
    {
        public static string ValidationSuccess(string ruleAttribute)
            => $"Validation SUCCESS => {ruleAttribute}";
        public static string ValidationError(string ruleAttribute, string attributeValue)
            => $"Validation ERROR => {ruleAttribute} => Value => {attributeValue} => BAD";

        public static string SaveSuccess(string version) => $"[{version}] Save SUCCESS";
        public static string SaveError(string version) => $"[{version}] Save ERROR";

        public static string SequenceSuccess(string sequenceName, string attribute)
            => $"{sequenceName} SUCCESS => {attribute}";
        public static string SequenceError(string sequenceName, string attribute)
            => $"{sequenceName} ERROR => {attribute}";
        public static string SequenceException(string sequenceName, Exception e)
            => $"{sequenceName} EXCEPTION => {e.GetFullMessage()}";

        public static string ModeStart(string modeName)
            => $"{modeName} START";
        public static string ModeEnd(string modeName)
            => $"{modeName} END";
        public static string ModeExecuted(string modeName)
            => $"{modeName} EXECUTED";

        public static string Off(MarketOrderType orderType)
            => $"{orderType.GetDescription()} => OFF";

        public static string BuyStepSuccess(decimal priceCurrent, decimal average, string change)
            => $"{MarketOrderType.BUYING.GetDescription()} " +
            $"=> CURRENT PRICE => ({priceCurrent}) " +
            $"=> DECREASED => CALCULATED AVERAGE " +
            $"=> ({average}) => BY ({change})";

        public static string BuyStepError(decimal priceCurrent, decimal average, string change, string needed)
            => $"{MarketOrderType.BUYING.GetDescription()} " +
            $"=> CURRENT PRICE => ({priceCurrent}) " +
            $"=> INCREASED => CALCULATED AVERAGE " +
            $"=> ({average}) => BY ({change}) " +
            $"=> NEEDED DECREASED CHANGE => ({needed})";

        public static string SellStepSuccess(SellType sellType, decimal priceCurrent, decimal average, string change)
            => $"{MarketOrderType.SELLING.GetDescription()} " +
            $"=> CURRENT PRICE => ({priceCurrent}) " +
            $"=> INCREASED => {sellType.GetDescription()} " +
            $"=> ({average}) => BY ({change})";

        public static string SellStepError(SellType sellType, decimal priceCurrent, decimal average, string change, string needed)
            => $"{MarketOrderType.SELLING.GetDescription()} " +
            $"=> CURRENT PRICE => ({priceCurrent}) " +
            $"=> DECREASED => {sellType.GetDescription()} " +
            $"=> ({average}) => BY ({change}) " +
            $"=> NEEDED INCREASED CHANGE => ({needed})";

        public static string StopLossStepSuccess(SellType sellType, decimal priceCurrent, decimal average, string change)
            => $"{MarketOrderType.STOPLOSS.GetDescription()} " +
            $"=> CURRENT PRICE => ({priceCurrent}) " +
            $"=> DECREASED => {sellType.GetDescription()} " +
            $"=> ({average}) => BY ({change})";

        public static string StopLossStepError(SellType sellType, decimal priceCurrent, decimal average, string change, string needed)
            => $"{MarketOrderType.STOPLOSS.GetDescription()} " +
            $"=> CURRENT PRICE => ({priceCurrent}) " +
            $"=> DECREASED => {sellType.GetDescription()} " +
            $"=> ({average}) => BY ({change}) " +
            $"=> NEEDED DECREASED CHANGE => ({needed})";

        public static string PriceMarketSuccess(MarketOrderType orderType)
            => $"{orderType.GetDescription()} => ORDER => EXCHANGE => PLACED";
        public static string PriceMarketError(MarketOrderType orderType)
            => $"{orderType.GetDescription()} => ORDER => EXCHANGE => NOT PLACED";

        public static string OrderMarketSuccess(MarketOrderType orderType)
            => $"{orderType.GetDescription()} => ORDER => EXCHANGE => SUCCEED";
        public static string OrderMarketError(MarketOrderType orderType, string message = "")
        {
            var result = string.IsNullOrWhiteSpace(message)
                ? string.Empty
                : $"=> {message}";

            return $"{orderType.GetDescription()} => ORDER => EXCHANGE => NOT SUCCEED {result}";
        }

        public static string TradeResultStart(long orderId)
            => $"ORDER ({orderId}) => START";
        public static string TradeResultEnd(long orderId, decimal average, decimal quantity, decimal commission)
            => $"ORDER ({orderId}) => END => AVERAGE => {average} => Quantity (all) => {quantity} => Commision (all) => {commission}";
        public static string TradeResult(MarketOrderType orderType, BinanceOrderTrade order)
            => $"{orderType.GetDescription()} => TRADE ({order.TradeId}) => Price => {order.Price} => Quantity {order.Quantity} => Commission {order.Commission} ({order.CommissionAsset})";

        public static string ExecuteMarketSuccess(MarketOrderType orderType, decimal bought)
        {
            return bought == 0
                ? $"{orderType.GetDescription()} => PRICE => REACHED"
                : $"{orderType.GetDescription()} => PRICE => REACHED => BOUGHT => ({bought})";
        }
        public static string ExecuteMarketError(MarketOrderType orderType, decimal bought)
        {
            return bought == 0
                ? $"{orderType.GetDescription()} => PRICE => NOT REACHED"
                : $"{orderType.GetDescription()} => PRICE => NOT REACHED => BOUGHT => ({bought})";
        }

        public static string ModeTypeSuccess(string sequenceName, string attribute)
            => $"{sequenceName} ON => {attribute}";
        public static string ModeTypeError(string sequenceName, string attribute)
            => $"{sequenceName} OFF => {attribute}";

        public static string AverageTypeSuccess(string sequenceName, string attribute)
            => $"{sequenceName} ON => {attribute}";
        public static string AverageTypeError(string sequenceName, string attribute)
            => $"{sequenceName} OFF => {attribute}";

        public static string ExchangeLog(string baseAsset, string quoteAsset, string error = "")
            => string.IsNullOrWhiteSpace(error)
                ? $"EXCHANGE ASSETS => BASE => ({baseAsset}) => QUOTE => ({quoteAsset})"
                : $"EXCHANGE ASSETS => ERROR => {error}";

        public static string NotificationTitleStart => "Hello! my dear Trader :)";
        public static string NotificationMessageStart
            => "I've started working for <strong>You</strong>.<br>" +
            " Join our <a href=\"https://t.me/joinchat/JmoiyRyhQp5o7Ts1ZezFQA\">Telegram Group</a>.<br><br>" +
            " Please visit <a href=\"https://cryptodev.tv\">https://cryptodev.tv</a>";

        public static string NotificationTitle(EnvironmentType workingType, MarketOrderType marketOrderType, string symbol)
            => $"[{workingType.GetDescription()}] => {marketOrderType.GetDescription()} [{symbol}]";
        public static string NotificationMessage(decimal average, decimal price, decimal percentage)
            => $"Average: {average}<br>Price: {price}<br>Change: {percentage}";
    }
}