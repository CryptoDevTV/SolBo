using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;

namespace SolBo.Shared.Rules.Order
{
    public class StopLossPriceReachedRule : IOrderRule
    {
        public string OrderStep => "StopLossPriceReached";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.StopLoss.PriceReached;

            var slPriceChange = solbot.BoughtPrice() - solbot.Communication.Price.Current > 0
                ? "falling"
                : "rising";

            var result = solbot.BoughtPrice() > 0
                ? $"100 - {solbot.Communication.Price.Current}(current) / {solbot.BoughtPrice()} * 100 = " +
                $"{100 - (solbot.Communication.Price.Current / solbot.BoughtPrice() * 100)}. (price {slPriceChange})." +
                $" stoplossdown => {solbot.Strategy.AvailableStrategy.StopLossDown}%"
                : "LAST BUY => NO";

            var slPrice = solbot.Strategy.AvailableStrategy.CommissionType == CommissionType.VALUE
                ? $"{solbot.Communication.Price.Current}(current) - {solbot.BoughtPrice()} = " +
                $"{solbot.Communication.Price.Current - solbot.BoughtPrice()}. (price {slPriceChange})." +
                $" stoplossdown => {solbot.Strategy.AvailableStrategy.StopLossDown}"
                : result;

            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"REACHED => {slPrice}"
                    : $"NOT REACHED => {slPrice}"
            };
        }
    }
}