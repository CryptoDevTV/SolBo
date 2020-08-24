using SolBo.Shared.Domain.Configs;
using SolBo.Shared.Domain.Enums;

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
                $"{solbot.Communication.Average.Current - solbot.Communication.Price.Current}. (price {buyPriceChange})" +
                $" Defined changed {solbot.Strategy.AvailableStrategy.BuyDown}"
                : "";

            return new OrderRuleResult
            {
                Success = response,
                Message = response
                    ? $"Bought price reached => {buyPrice}"
                    : $"Bought price not reached => {buyPrice}"
            };
        }
    }
}