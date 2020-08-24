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

            var slPriceChange = solbot.Communication.Average.Current - solbot.Communication.Price.Current > 0
                ? "falling"
                : "rising";

            var slPrice = solbot.Strategy.AvailableStrategy.CommissionType == CommissionType.VALUE
                ? $"{solbot.BoughtPrice()} - {solbot.Communication.Price.Current}(current) = " +
                $"{solbot.BoughtPrice() - solbot.Communication.Price.Current}. (price {slPriceChange})" +
                $" Defined changed {solbot.Strategy.AvailableStrategy.StopLossDown}"
                : "";

            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"Stop loss price reached => {slPrice}"
                    : $"Stop loss price not reached => {slPrice}"
            };
        }
    }
}