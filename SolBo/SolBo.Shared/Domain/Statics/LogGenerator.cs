using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
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

        public static string ModeStart(string modeName)
            => $"{modeName} START";
        public static string ModeEnd(string modeName)
            => $"{modeName} END";
        public static string ModeExecuted(string modeName)
            => $"{modeName} EXECUTED";

        public static string Off(MarketOrderType orderType)
            => $"{orderType.GetDescription()} => OFF";

        public static string StepMarketSuccess(MarketOrderType orderType, decimal priceCurrent, decimal priceAverage, decimal change)
            => $"{orderType.GetDescription()} => Price ({priceCurrent}) increased from the average ({priceAverage}) by {Math.Abs(change)}%";
        public static string StepMarketError(MarketOrderType orderType, decimal priceCurrent, decimal priceAverage, decimal change)
            => $"{orderType.GetDescription()} => Price ({priceCurrent}) has fallen from the average ({priceAverage}) by {Math.Abs(change)}%";

        public static string ExecuteMarketSuccess(MarketOrderType orderType, bool priceReached, int bought)
            => $"{orderType.GetDescription()} => Price reached ({priceReached}), bought before ({bought})";
        public static string ExecuteMarketError(MarketOrderType orderType, bool priceReached, int bought)
            => $"{orderType.GetDescription()} => Price reached ({priceReached}), bought before ({bought})";
    }
}