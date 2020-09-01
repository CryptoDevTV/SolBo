using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;
using System;

namespace SolBo.Shared.Rules.Order
{
    public class SellPriceReachedRule : IOrderRule
    {
        public string OrderStep => "SellPriceReached";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.Sell.PriceReached;

            var sellPriceChange = solbot.BoughtPrice() - solbot.Communication.Price.Current > 0
                ? "falling"
                : "rising";

            var result = solbot.BoughtPrice() > 0
                ? $"100 - {solbot.Communication.Price.Current}(current) / {solbot.BoughtPrice()} * 100 = " +
                $"{Math.Round(100 - (solbot.Communication.Price.Current / solbot.BoughtPrice() * 100), 2)}. (price {sellPriceChange})." +
                $" => sellup => {solbot.Strategy.AvailableStrategy.SellUp}%"
                : "LAST BUY => NO";

            var sellPrice = solbot.Strategy.AvailableStrategy.CommissionType == CommissionType.VALUE
                ? $"{solbot.Communication.Price.Current}(current) - {solbot.BoughtPrice()} = " +
                $"{Math.Round(solbot.Communication.Price.Current - solbot.BoughtPrice(), 2)}. (price {sellPriceChange})." +
                $" => sellup => {solbot.Strategy.AvailableStrategy.SellUp}"
                : result;

            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"REACHED => {sellPrice}"
                    : $"NOT REACHED => {sellPrice}"
            };
        }
    }
}