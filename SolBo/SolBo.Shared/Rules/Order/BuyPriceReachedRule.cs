using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Domain.Statics;
using System;

namespace SolBo.Shared.Rules.Order
{
    public class BuyPriceReachedRule : IOrderRule
    {
        public string OrderStep => "BuyPriceReached";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.Buy.PriceReached;

            var buyPriceChange = solbot.Communication.Average.Current - solbot.Communication.Price.Current > 0
                ? "falling"
                : "rising";

            var buyPrice = solbot.Strategy.AvailableStrategy.CommissionType == CommissionType.VALUE
                ? $"{solbot.Communication.Average.Current} - {solbot.Communication.Price.Current}(current) = " +
                $"{solbot.Communication.Average.Current - solbot.Communication.Price.Current}. (price {buyPriceChange})." +
                $" => buydown => {solbot.Strategy.AvailableStrategy.BuyDown}"
                : $"100 - ({solbot.Communication.Price.Current} / {solbot.Communication.Average.Current} * 100) = " + 
                $"{Math.Round(100 - (solbot.Communication.Price.Current / solbot.Communication.Average.Current * 100), GlobalConfig.RoundValue)}. (price {buyPriceChange})." +
                $" => buydown => {solbot.Strategy.AvailableStrategy.BuyDown}%";

            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"REACHED => {buyPrice}"
                    : $"NOT REACHED => {buyPrice}"
            };
        }
    }
}