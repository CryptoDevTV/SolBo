using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;
using SolBo.Shared.Extensions;

namespace SolBo.Shared.Rules.Order
{
    public class SellPriceReachedRule : IOrderRule
    {
        public string OrderStep => "SellPriceReached";
        public IRuleResult RuleExecuted(Solbot solbot)
        {
            var response = solbot.Communication.Sell.PriceReached;

            var sellPriceChange = solbot.Communication.Average.Current - solbot.Communication.Price.Current > 0
                ? "falling"
                : "rising";

            var sellPrice = solbot.Strategy.AvailableStrategy.CommissionType == CommissionType.VALUE
                ? $"{solbot.BoughtPrice()} - {solbot.Communication.Price.Current}(current) = " +
                $"{solbot.BoughtPrice() - solbot.Communication.Price.Current}. (price {sellPriceChange})" +
                $" Defined changed {solbot.Strategy.AvailableStrategy.SellUp}"
                : "";

            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"Sell price reached => {sellPrice}"
                    : $"Sell price not reached => {sellPrice}"
            };
        }
    }
}